#include "Pch_VulkanRenderer.h"
#include "RendererImpl.h"
#include "Utils\DebugAssert.h"
#include "Utils\UuidGenerator.h"
#include "Utils\Logger.h"
#include "Utils\Throw.h"
#include "VulkanUtils.h"
#include "Instance.h"
#include "PhysicalDevice.h"
#include "Surface.h"

namespace wc = Sisyphus::WindowCreator;

namespace Sisyphus::Rendering::Vulkan {

	constexpr uint64_t timeout = 100000000; // 100ms

	RendererImpl::RendererImpl(const RendererCreateInfo& ci) :
		ci(ci),
		device(nullptr),
		commandPool(nullptr),
		swapchain(nullptr),
		depthBuffer(nullptr),
		descriptorSetLayout(nullptr),
		pipelineLayout(nullptr),
		descriptorPool(nullptr),
		descriptorSet(nullptr),
		uniformBuffer(nullptr),
		renderPass(nullptr),
		vertexBuffer(nullptr),
		pipeline(nullptr),
		logger(&Logger::Get())
	{
		if (logger == nullptr) {
			SIS_THROW("Logger not found");
		}

		EnumerateInstanceLayerProperties();

		componentManager.InitComponent<Instance>();
		componentManager.InitComponent<PhysicalDevice>();
		componentManager.InitComponent<Surface>(ci.window);

		InitDevice();
		logger->Log("Vulkan Device initialized!");
		InitCommandPool();
		logger->Log("Command Pool initialized!");
		InitSwapchain();
		logger->Log("Swapchain initialized!");
		InitSwapchainImages();
		logger->Log(std::to_string(swapchainImages.size()) + " Swapchain Images initialized!");
		InitImageViews();
		logger->Log(std::to_string(imageViews.size()) + " Image Views initialized!");
		
		logger->BeginSection("Depth Buffer");
		InitDepthBuffer();
		logger->Log("Depth Buffer initialized!");
		logger->EndSection();

		InitDescriptorSetLayout();
		logger->Log("Descriptor Set Layout initialized!");
		InitPipelineLayout();
		logger->Log("Pipeline Layout initialized!");
		InitDescriptorPool();
		logger->Log("Descriptor Pool initialized!");
		InitDescriptorSet();
		logger->Log("Descriptor Set initialized!");

		logger->BeginSection("Uniform Buffer");
		InitUniformBuffer();
		logger->Log("Uniform Buffer initialized!");
		logger->EndSection();

		InitRenderPass();
		logger->Log("Render Pass initialized!");

		InitFramebuffers();
		logger->Log(std::to_string(framebuffers.size()) + " Framebuffers initialized!");		

		InitShaders();
	}

	RendererImpl::~RendererImpl() = default;

	void RendererImpl::Draw(const IDrawable & drawable)
	{
		AdaptToSurfaceChanges();

		SIS_DEBUGASSERT(device);
		SIS_DEBUGASSERT(!framebuffers.empty());
		SIS_DEBUGASSERT(renderPass);
		SIS_DEBUGASSERT(descriptorSet);
		vk::Extent2D surfaceExtent = componentManager.GetComponent<Surface>().GetExtent();

		InitPipeline(drawable.GetVertexStride());
		SIS_DEBUGASSERT(pipeline);

		InitVertexBuffer(drawable.GetVertexBufferSize());
		SIS_DEBUGASSERT(vertexBuffer);
		vertexBuffer->GetDeviceData().Set(drawable.GetVertexData());

		vk::UniqueSemaphore imageAcquiredSemaphore = device->createSemaphoreUnique(vk::SemaphoreCreateInfo{});
		auto currentBuffer = device->acquireNextImageKHR(*swapchain, timeout, *imageAcquiredSemaphore, nullptr);

		SIS_THROWASSERT_MSG(currentBuffer.result == vk::Result::eSuccess, "Failed to acquire an image buffer!");
		SIS_THROWASSERT_MSG(
			currentBuffer.value < framebuffers.size(),
			"Acquired image index (" + std::to_string(currentBuffer.value) +
			") higher that the number of available framebuffers (" + std::to_string(framebuffers.size()) + ")");

		InitCommandBuffers();
		SIS_DEBUGASSERT(!commandBuffers.empty());
		auto& commandBuffer = commandBuffers[0];
		commandBuffer->begin(vk::CommandBufferBeginInfo{});

		vk::ClearValue clearValues[2];
		clearValues[0].color = vk::ClearColorValue(std::array<float, 4>{0.9f, 0.8f, 0.7f, 0.9f});
		clearValues[1].depthStencil = vk::ClearDepthStencilValue(1.0f, 0);
		vk::RenderPassBeginInfo renderPassBeginInfo{
			*renderPass,
			*framebuffers[currentBuffer.value],
			vk::Rect2D(vk::Offset2D(0, 0), surfaceExtent),
			2,
			clearValues
		};
		commandBuffer->beginRenderPass(renderPassBeginInfo, vk::SubpassContents::eInline);
		commandBuffer->bindPipeline(vk::PipelineBindPoint::eGraphics, *pipeline);
		commandBuffer->bindDescriptorSets(vk::PipelineBindPoint::eGraphics, *pipelineLayout, 0, *descriptorSet, nullptr);
		commandBuffer->bindVertexBuffers(0, vertexBuffer->GetBuffer(), { 0 });
		commandBuffer->setViewport(0, vk::Viewport(
			0.0f, 0.0f, 
			static_cast<float>(surfaceExtent.width), static_cast<float>(surfaceExtent.height),
			0.0f, 1.0f));
		commandBuffer->setScissor(0, vk::Rect2D(vk::Offset2D(0, 0), surfaceExtent));

		commandBuffer->draw(drawable.GetVertexCount(), 1, 0, 0);
		commandBuffer->endRenderPass();
		commandBuffer->end();

		vk::UniqueFence drawFence = device->createFenceUnique({});

		vk::PipelineStageFlags waitDestinationStageMask(vk::PipelineStageFlagBits::eColorAttachmentOutput);
		vk::SubmitInfo submitInfo(1, &*imageAcquiredSemaphore, &waitDestinationStageMask, 1, &*commandBuffer);

		auto physicalDevice = componentManager.GetComponent<PhysicalDevice>();
		vk::Queue graphicsQueue = device->getQueue(physicalDevice.GetGraphicsQueueFamilyIndex(), 0);
		vk::Queue presentQueue = device->getQueue(physicalDevice.GetPresentQueueFamilyIndex(), 0);

		graphicsQueue.submit(submitInfo, *drawFence);

		auto waitResult = device->waitForFences(*drawFence, VK_TRUE, timeout);
		if (waitResult == vk::Result::eTimeout) {
			logger->Log("Draw timeout!");
		}
		else if (waitResult == vk::Result::eSuccess) {
			logger->Log("Draw successful!");
		}
		else {
			SIS_DEBUGASSERT_MSG(false, "Unexpected Draw result!");
		}
		vk::PresentInfoKHR presentInfo{
			0,
			nullptr,
			1,
			&*swapchain,
			&currentBuffer.value
		};
		presentQueue.presentKHR(presentInfo);

		device->resetCommandPool(*commandPool, {});
	}

	void RendererImpl::InitDevice()
	{
		auto physicalDevice = componentManager.GetComponent<PhysicalDevice>();
		auto deviceQueueCreateInfos = physicalDevice.GetDeviceQueueCreateInfos();

		std::vector<const char*> deviceExtensionNames;
		deviceExtensionNames.push_back(VK_KHR_SWAPCHAIN_EXTENSION_NAME);

		vk::DeviceCreateInfo deviceCreateInfo(
			{},
			static_cast<uint32_t>(deviceQueueCreateInfos.size()),
			deviceQueueCreateInfos.data(),
			0,
			nullptr,
			static_cast<uint32_t>(deviceExtensionNames.size()),
			deviceExtensionNames.data()
		);
		device = physicalDevice.GetVulkanObject().createDeviceUnique(deviceCreateInfo);
	}

	void RendererImpl::InitCommandPool()
	{
		SIS_DEBUGASSERT(device);

		vk::CommandPoolCreateInfo commandPoolCreateInfo(
			{},
			componentManager.GetComponent<PhysicalDevice>().GetGraphicsQueueFamilyIndex()
		);
		commandPool = device->createCommandPoolUnique(commandPoolCreateInfo);
	}

	void RendererImpl::InitCommandBuffers()
	{
		SIS_DEBUGASSERT(device);
		SIS_DEBUGASSERT(commandPool);

		vk::CommandBufferAllocateInfo commandBufferAllocateInfo(
			*commandPool,
			vk::CommandBufferLevel::ePrimary,
			1
		);
		commandBuffers = device->allocateCommandBuffersUnique(commandBufferAllocateInfo);
	}

	void RendererImpl::InitSwapchain()
	{
		auto& surface = componentManager.GetComponent<Surface>();

		constexpr int desiredMinImageCount = 3; // triple buffering
		auto physicalDevice = componentManager.GetComponent<PhysicalDevice>().GetVulkanObject();
		auto surfaceCapabilites = physicalDevice.getSurfaceCapabilitiesKHR(surface);
		logger->Log("Surface minImageCount: " + std::to_string(surfaceCapabilites.minImageCount));
		logger->Log("Surface maxImageCount: " + std::to_string(surfaceCapabilites.maxImageCount));
		if (
			surfaceCapabilites.minImageCount > desiredMinImageCount ||
			surfaceCapabilites.maxImageCount < desiredMinImageCount)
		{
			SIS_THROW("Surface does not support three image buffers");
		}

		logger->Log("Surface extent: (w: " + std::to_string(surfaceCapabilites.currentExtent.width) +
			", h: " +  std::to_string(surfaceCapabilites.currentExtent.height) + ")");

		if (!(surfaceCapabilites.supportedTransforms & vk::SurfaceTransformFlagBitsKHR::eIdentity)) {
			SIS_THROW("Identity surface transform not supported");
		}

		logger->Log("Supported composite alpha: " + vk::to_string(surfaceCapabilites.supportedCompositeAlpha));
		if (!(surfaceCapabilites.supportedCompositeAlpha & vk::CompositeAlphaFlagBitsKHR::eOpaque)) {
			SIS_THROW("Surface opaque composite alpha mode not supported");
		}

		auto desiredPresentMode = vk::PresentModeKHR::eFifoRelaxed;
		auto supportedPresentModes = physicalDevice.getSurfacePresentModesKHR(surface);
		bool modeFound = false;
		logger->Log("Supported present modes: ");
		for (auto&& mode : supportedPresentModes) {
			logger->Log(vk::to_string(mode));
			if (mode == desiredPresentMode) {
				modeFound = true;
			}
		}
		if (!modeFound) {
			SIS_THROW("Present mode " + vk::to_string(desiredPresentMode) + " not supported by GPU");
		}

		auto format = surface.GetFormat();
		auto colorSpace = surface.GetColorSpace();

		vk::SwapchainCreateInfoKHR swapchainCreateInfo(
			{},
			surface,
			desiredMinImageCount,
			format,
			colorSpace,
			surfaceCapabilites.currentExtent,
			1,
			vk::ImageUsageFlagBits::eColorAttachment,
			vk::SharingMode::eExclusive,
			0,
			nullptr,
			vk::SurfaceTransformFlagBitsKHR::eIdentity,
			vk::CompositeAlphaFlagBitsKHR::eOpaque,
			desiredPresentMode,
			false,
			nullptr
		);

		swapchain = device->createSwapchainKHRUnique(swapchainCreateInfo);
	}

	void RendererImpl::InitSwapchainImages()
	{
		SIS_DEBUGASSERT(swapchain);
		swapchainImages = device->getSwapchainImagesKHR(*swapchain);
	}

	void RendererImpl::InitImageViews()
	{
		SIS_DEBUGASSERT(!swapchainImages.empty());

		vk::ComponentMapping componentMapping{
			vk::ComponentSwizzle::eR,
			vk::ComponentSwizzle::eG,
			vk::ComponentSwizzle::eB,
			vk::ComponentSwizzle::eA,
		};
		vk::ImageSubresourceRange subresourceRange{
			vk::ImageAspectFlagBits::eColor,
			0,
			1,
			0,
			1
		};
		imageViews.reserve(swapchainImages.size());
		for (auto&& image : swapchainImages) {
			vk::ImageViewCreateInfo imageViewCreateInfo(
				{},
				image,
				vk::ImageViewType::e2D,
				componentManager.GetComponent<Surface>().GetFormat(),
				componentMapping,
				subresourceRange
			);
			imageViews.emplace_back(device->createImageViewUnique(imageViewCreateInfo));
		}
	}

	void RendererImpl::InitDepthBuffer()
	{
		SIS_DEBUGASSERT(device);

		auto physicalDevice = componentManager.GetComponent<PhysicalDevice>().GetVulkanObject();

		DepthBuffer::CreateInfo createInfo{
			componentManager.GetComponent<Surface>().GetExtent(),
			physicalDevice,
			*device,
			logger
		};

		depthBuffer = std::make_unique<DepthBuffer>(createInfo);
	}

	void RendererImpl::InitDescriptorSetLayout()
	{
		SIS_DEBUGASSERT(device);

		vk::DescriptorSetLayoutBinding binding{
			0,
			vk::DescriptorType::eUniformBuffer,
			1,
			vk::ShaderStageFlagBits::eVertex
		};

		descriptorSetLayout = device->createDescriptorSetLayoutUnique(vk::DescriptorSetLayoutCreateInfo(
			{},
			1,
			&binding
		));
	}

	void RendererImpl::InitPipelineLayout()
	{
		SIS_DEBUGASSERT(descriptorSetLayout);
		SIS_DEBUGASSERT(device);

		pipelineLayout = device->createPipelineLayoutUnique(vk::PipelineLayoutCreateInfo(
			{},
			1,
			&(*descriptorSetLayout)
		));		
	}

	void RendererImpl::InitDescriptorPool()
	{
		SIS_DEBUGASSERT(device);

		vk::DescriptorPoolSize poolSize(vk::DescriptorType::eUniformBuffer, 1);

		vk::DescriptorPoolCreateInfo descriptorPoolCreateInfo(
			{vk::DescriptorPoolCreateFlagBits::eFreeDescriptorSet},
			1,
			1,
			&poolSize
		);
		
		descriptorPool = device->createDescriptorPoolUnique(descriptorPoolCreateInfo);
	}

	void RendererImpl::InitDescriptorSet()
	{
		SIS_DEBUGASSERT(device);
		SIS_DEBUGASSERT(descriptorPool);

		vk::DescriptorSetAllocateInfo allocateInfo(
			*descriptorPool,
			1,
			&*descriptorSetLayout
		);

		descriptorSet = std::move(device->allocateDescriptorSetsUnique(allocateInfo).front());
	}

	void RendererImpl::InitUniformBuffer()
	{
		SIS_DEBUGASSERT(device);
		SIS_DEBUGASSERT(descriptorSet);

		auto physicalDevice = componentManager.GetComponent<PhysicalDevice>().GetVulkanObject();

		UniformBuffer::CreateInfo createInfo{
			sizeof(Renderer::UniformBufferData),
			*device,
			physicalDevice,
			*descriptorSet,
			logger
		};

		uniformBuffer = std::make_unique<UniformBuffer>(createInfo);
	}

	void RendererImpl::InitRenderPass()
	{
		SIS_DEBUGASSERT(device);

		vk::AttachmentDescription attachmentDescriptions[2];

		// color
		attachmentDescriptions[0] = vk::AttachmentDescription(
			{},
			componentManager.GetComponent<Surface>().GetFormat(),
			vk::SampleCountFlagBits::e1,
			vk::AttachmentLoadOp::eClear,
			vk::AttachmentStoreOp::eStore,
			vk::AttachmentLoadOp::eDontCare,
			vk::AttachmentStoreOp::eDontCare,
			vk::ImageLayout::eUndefined,
			vk::ImageLayout::ePresentSrcKHR
		);

		// depth
		attachmentDescriptions[1] = vk::AttachmentDescription(
			{},
			DepthBuffer::format,
			vk::SampleCountFlagBits::e1,
			vk::AttachmentLoadOp::eClear,
			vk::AttachmentStoreOp::eDontCare,
			vk::AttachmentLoadOp::eDontCare,
			vk::AttachmentStoreOp::eDontCare,
			vk::ImageLayout::eUndefined,
			vk::ImageLayout::eDepthStencilAttachmentOptimal
		);

		vk::AttachmentReference colorReference(0, vk::ImageLayout::eColorAttachmentOptimal);
		vk::AttachmentReference depthReference(1, vk::ImageLayout::eDepthStencilAttachmentOptimal);

		vk::SubpassDescription subpassDescription(
			{},
			vk::PipelineBindPoint::eGraphics,
			0,
			nullptr,
			1,
			&colorReference,
			nullptr,
			&depthReference,
			0,
			nullptr
		);

		vk::RenderPassCreateInfo renderPassCreateInfo(
			{},
			2,
			attachmentDescriptions,
			1,
			&subpassDescription
		);

		renderPass = device->createRenderPassUnique(renderPassCreateInfo);
	}

	void RendererImpl::InitFramebuffers()
	{
		SIS_DEBUGASSERT(!imageViews.empty());
		SIS_DEBUGASSERT(depthBuffer);
		SIS_DEBUGASSERT(device);
		auto surfaceExtent = componentManager.GetComponent<Surface>().GetExtent();
		vk::ImageView attachments[2];
		attachments[1] = depthBuffer->GetImageView();

		for (const auto& imageView : imageViews) {
			attachments[0] = *imageView;
			vk::FramebufferCreateInfo framebufferCreateInfo{
				{},
				*renderPass,
				2,
				attachments,
				surfaceExtent.width,
				surfaceExtent.height,
				1
			};

			framebuffers.push_back(device->createFramebufferUnique(framebufferCreateInfo));
		}
	}

	void RendererImpl::InitShaders()
	{
		for (const auto& shaderInfo : ci.shaders) {
			CreateShader(shaderInfo);
			if (vertexShaderId.is_nil() && shaderInfo.type == ShaderType::Vertex) {
				EnableShader(shaderInfo.id);
			}
			if (fragmentShaderId.is_nil() && shaderInfo.type == ShaderType::Fragment) {
				EnableShader(shaderInfo.id);
			}
		}
		if (vertexShaderId.is_nil()) {
			SIS_THROW("Unable to find a vertex shader");
		}
		if (fragmentShaderId.is_nil()) {
			SIS_THROW("Unable to find a fragment shader");
		}
	}

	void RendererImpl::AdaptToSurfaceChanges()
	{
		auto physicalDevice = componentManager.GetComponent<PhysicalDevice>().GetVulkanObject();
		auto& surface = componentManager.GetComponent<Surface>();

		auto surfaceCapabilities = physicalDevice.getSurfaceCapabilitiesKHR(surface);
		bool surfaceChanged = surfaceCapabilities.currentExtent != surface.GetExtent();
		if (surfaceChanged) {
			logger->Log(
				"Surface extent changed from " + ToString(surface.GetExtent()) +
				" to " + ToString(surfaceCapabilities.currentExtent));
			__debugbreak();
		}
	}

	void RendererImpl::InitVertexBuffer(size_t size)
	{
		logger->BeginSection("Vertex Buffer");
		SIS_DEBUGASSERT(device);

		auto physicalDevice = componentManager.GetComponent<PhysicalDevice>().GetVulkanObject();

		vertexBuffer = std::make_unique<VertexBuffer>(VertexBuffer::CreateInfo{
			size,
			*device,
			physicalDevice
		});

		logger->Log("Vertex Buffer of size " + std::to_string(size) + " initialized!");
		logger->EndSection();
	}

	void RendererImpl::InitPipeline(uint32_t stride)
	{
		SIS_DEBUGASSERT(pipelineLayout);
		SIS_DEBUGASSERT(renderPass);
		SIS_DEBUGASSERT(device);

		if (!ShaderExists(vertexShaderId)) {
			SIS_THROW("Vertex shader not found");
		}
		if (!ShaderExists(fragmentShaderId)) {
			SIS_THROW("Fragment shader not found");
		}

		vk::PipelineShaderStageCreateInfo shaderStageCreateInfos[2]{
			vk::PipelineShaderStageCreateInfo{
				{},
				vk::ShaderStageFlagBits::eVertex,
				GetShader(vertexShaderId).GetModule(),
				"main"
			},
			vk::PipelineShaderStageCreateInfo{
				{},
				vk::ShaderStageFlagBits::eFragment,
				GetShader(fragmentShaderId).GetModule(),
				"main"
			}
		};

		vk::VertexInputBindingDescription vertexInputBindingDescription(0, stride);
		vk::VertexInputAttributeDescription vertexInputAttributeDescription(0, 0, vk::Format::eR32G32B32Sfloat);
		vk::PipelineVertexInputStateCreateInfo pipelineVertexInputStateCreateInfo{
			{},
			1,
			&vertexInputBindingDescription,
			1,
			&vertexInputAttributeDescription
		};

		vk::PipelineInputAssemblyStateCreateInfo pipelineInputAssemblyStateCreateInfo{
			{},
			vk::PrimitiveTopology::eTriangleList
		};

		vk::PipelineViewportStateCreateInfo pipelineViewportStateCreateInfo{
			{},
			1,
			nullptr,
			1,
			nullptr
		};

		vk::PipelineRasterizationStateCreateInfo pipelineRasterizationStateCreateInfo{
			{},
			false,
			false,
			vk::PolygonMode::eFill,
			vk::CullModeFlagBits::eBack,
			vk::FrontFace::eClockwise,
			false,
			0.0f,
			0.0f,
			0.0f,
			1.0f
		};

		vk::PipelineMultisampleStateCreateInfo pipelineMultisampleStateCreateInfo;

		vk::PipelineDepthStencilStateCreateInfo pipelineDepthStencilStateCreateInfo{
			{},
			true,
			true,
			vk::CompareOp::eLessOrEqual
		};

		vk::ColorComponentFlags colorComponentFlags(vk::ColorComponentFlagBits::eR | vk::ColorComponentFlagBits::eG | vk::ColorComponentFlagBits::eB | vk::ColorComponentFlagBits::eA);
		vk::PipelineColorBlendAttachmentState pipelineColorBlendAttachmentState{
			false,
			vk::BlendFactor::eZero,
			vk::BlendFactor::eZero,
			vk::BlendOp::eAdd,
			vk::BlendFactor::eZero,
			vk::BlendFactor::eZero,
			vk::BlendOp::eAdd,
			colorComponentFlags
		};
		vk::PipelineColorBlendStateCreateInfo pipelineColorBlendStateCreateInfo{
			{},
			false,
			vk::LogicOp::eNoOp,
			1,
			&pipelineColorBlendAttachmentState,
			{{(1.0f, 1.0f, 1.0f, 1.0f)}}
		};

		std::vector<vk::DynamicState> dynamicStates{ vk::DynamicState::eViewport, vk::DynamicState::eScissor };
		vk::PipelineDynamicStateCreateInfo pipelineDynamicStateCreateInfo{
			{},
			static_cast<uint32_t>(dynamicStates.size()),
			dynamicStates.data()
		};

		vk::GraphicsPipelineCreateInfo graphicsPipelineCreateInfo{
			{},
			2,
			shaderStageCreateInfos,
			&pipelineVertexInputStateCreateInfo,
			&pipelineInputAssemblyStateCreateInfo,
			nullptr,
			&pipelineViewportStateCreateInfo,
			&pipelineRasterizationStateCreateInfo,
			&pipelineMultisampleStateCreateInfo,
			&pipelineDepthStencilStateCreateInfo,
			&pipelineColorBlendStateCreateInfo,
			&pipelineDynamicStateCreateInfo,
			*pipelineLayout,
			*renderPass
		};

		pipeline = device->createGraphicsPipelineUnique(nullptr, graphicsPipelineCreateInfo);		
	}

	Shader& RendererImpl::GetShader(uuids::uuid id)
	{
		if (!ShaderExists(id)) {
			SIS_THROW("Shader " + uuids::to_string(id) + " not found");
		}
		return *shaders[id];
	}

	void RendererImpl::CreateShader(const ShaderInfo& shaderInfo)
	{
		SIS_DEBUGASSERT(device);

		Shader::CreateInfo shaderCreateInfo{
			shaderInfo.code,
			shaderInfo.type,
			*device
		};
		auto shader = std::make_unique<Shader>(shaderCreateInfo);
		auto id = shaderInfo.id;
		shaders[id] = std::move(shader);

		logger->Log("Shader " + uuids::to_string(id) + " created!");
	}

	bool RendererImpl::ShaderExists(uuids::uuid id) const
	{
		return shaders.find(id) != shaders.end();
	}

	void RendererImpl::EnableShader(uuids::uuid id)
	{
		if (!ShaderExists(id)) {
			SIS_THROW("Shader " + uuids::to_string(id) + " does not exist");
		}
		auto type = shaders[id]->GetType();
		switch (type) {
		case ShaderType::Vertex:
			vertexShaderId = id;
			break;
		case ShaderType::Fragment:
			fragmentShaderId = id;
			break;
		default:
			SIS_DEBUGASSERT(false);
			break;
		}

		logger->Log("Shader " + uuids::to_string(id) + " enabled!");
	}

	void RendererImpl::UpdateUniformBuffer(Renderer::UniformBufferData data)
	{
		SIS_DEBUGASSERT(uniformBuffer);
		uniformBuffer->UpdateData(data);
	}

	Renderer::UniformBufferData RendererImpl::GetUniformBufferData()
	{
		SIS_DEBUGASSERT(uniformBuffer);
		return uniformBuffer->GetData<Renderer::UniformBufferData>();
	}

}