#include <vector>
#include <string>
#include <iostream>
#include <sstream>
#include "RendererImpl.h"
#include "WindowCreator\WindowCreator.h"
#include "Utils\DebugAssert.h"
#include "Utils\UuidGenerator.h"
#include "Utils\Logger.h"
#include "Utils\Throw.h"
#include "VulkanUtils.h"
#include "Instance.h"

namespace wc = Sisyphus::WindowCreator;

namespace Sisyphus::Rendering::Vulkan {

	constexpr uint64_t timeout = 100000000; // 100ms

	RendererImpl::RendererImpl(const RendererCreateInfo& ci) :
		ci(ci),
		windowExtent(nullptr),
		surface(nullptr),
		physicalDevice(nullptr),
		queueFamilyIndex(std::nullopt),
		device(nullptr),
		commandPool(nullptr),
		colorFormat(std::nullopt),
		colorSpace(std::nullopt),
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

		componentManager.InitComponent(ComponentType::Instance);

		InitWindowExtent();
		logger->Log("Window extent initialized!");
		InitSurface();
		logger->Log("Surface initialized!");
		InitPhysicalDevice();
		logger->Log("Physical Device initialized!");
		InitQueueFamilyIndex();
		logger->Log("Queue Family Index initialized!");
		InitDevice();
		logger->Log("Vulkan Device initialized!");
		InitCommandPool();
		logger->Log("Command Pool initialized!");
		InitFormatAndColorSpace();
		logger->Log("Format initialized: " + vk::to_string(colorFormat.value()));
		logger->Log("Color space initialized: " + vk::to_string(colorSpace.value()));
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
		SIS_DEBUGASSERT(windowExtent);

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
			vk::Rect2D(vk::Offset2D(0, 0), GetExtent2D(*windowExtent)),
			2,
			clearValues
		};
		commandBuffer->beginRenderPass(renderPassBeginInfo, vk::SubpassContents::eInline);
		commandBuffer->bindPipeline(vk::PipelineBindPoint::eGraphics, *pipeline);
		commandBuffer->bindDescriptorSets(vk::PipelineBindPoint::eGraphics, *pipelineLayout, 0, *descriptorSet, nullptr);
		commandBuffer->bindVertexBuffers(0, vertexBuffer->GetBuffer(), { 0 });
		commandBuffer->setViewport(0, vk::Viewport(
			0.0f, 0.0f, 
			static_cast<float>(windowExtent->width), static_cast<float>(windowExtent->height), 
			0.0f, 1.0f));
		commandBuffer->setScissor(0, vk::Rect2D(vk::Offset2D(0, 0), GetExtent2D(*windowExtent)));

		commandBuffer->draw(drawable.GetVertexCount(), 1, 0, 0);
		commandBuffer->endRenderPass();
		commandBuffer->end();

		vk::UniqueFence drawFence = device->createFenceUnique({});

		vk::PipelineStageFlags waitDestinationStageMask(vk::PipelineStageFlagBits::eColorAttachmentOutput);
		vk::SubmitInfo submitInfo(1, &*imageAcquiredSemaphore, &waitDestinationStageMask, 1, &*commandBuffer);

		// assuming that the queue support both graphics and present operations for now
		vk::Queue graphicsQueue = device->getQueue(queueFamilyIndex.value(), 0);
		vk::Queue presentQueue = device->getQueue(queueFamilyIndex.value(), 0);

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

	void RendererImpl::InitWindowExtent()
	{
		windowExtent = std::make_unique<WindowCreator::WindowExtent>(ci.window->GetExtent());
	}

	void RendererImpl::InitSurface()
	{
		SIS_DEBUGASSERT(ci.window);
		surface = ci.window->GetVulkanSurface(componentManager.GetComponent<Instance>());
	}

	void RendererImpl::InitPhysicalDevice()
	{
		auto physicalDevices = componentManager.GetComponent<Instance>().GetVulkanObject().enumeratePhysicalDevices();
		if (physicalDevices.empty()) {
			throw::std::runtime_error("No physical devices supporting Vulkan");
		}

		physicalDevice = physicalDevices[0];
		logger->Log("Creating a Vulkan Device from " + std::string(physicalDevice.getProperties().deviceName));
		InspectDevice(physicalDevice, logger);
	}

	void RendererImpl::InitQueueFamilyIndex()
	{
		SIS_DEBUGASSERT(physicalDevice);
		SIS_DEBUGASSERT(surface);
		queueFamilyIndex = FindGraphicsQueueFamilyIndex(physicalDevice, *surface);
		if (queueFamilyIndex == -1) {
			SIS_THROW("Graphics queue not found in the device");
		}
		logger->Log("Choosing queue family #" + std::to_string(queueFamilyIndex.value()));
	}

	void RendererImpl::InitDevice()
	{
		SIS_DEBUGASSERT(physicalDevice);
		SIS_DEBUGASSERT(queueFamilyIndex);

		vk::DeviceQueueCreateInfo deviceQueueCreateInfo(
			{},
			queueFamilyIndex.value(),
			1
		);

		std::vector<const char*> deviceExtensionNames;
		deviceExtensionNames.push_back(VK_KHR_SWAPCHAIN_EXTENSION_NAME);

		vk::DeviceCreateInfo deviceCreateInfo(
			{},
			1,
			&deviceQueueCreateInfo,
			0,
			nullptr,
			static_cast<uint32_t>(deviceExtensionNames.size()),
			deviceExtensionNames.data()
		);
		device = physicalDevice.createDeviceUnique(deviceCreateInfo);
	}

	void RendererImpl::InitCommandPool()
	{
		SIS_DEBUGASSERT(queueFamilyIndex);
		SIS_DEBUGASSERT(device);

		vk::CommandPoolCreateInfo commandPoolCreateInfo(
			{},
			queueFamilyIndex.value()
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

	void RendererImpl::InitFormatAndColorSpace()
	{
		SIS_DEBUGASSERT(surface);

		constexpr vk::Format desiredFormat = vk::Format::eB8G8R8A8Srgb;
		constexpr vk::ColorSpaceKHR desiredColorSpace = vk::ColorSpaceKHR::eSrgbNonlinear;
		bool formatFound = false;

		auto surfaceFormats = physicalDevice.getSurfaceFormatsKHR(*surface);
		logger->BeginSection("Surface formats:");
		for (int i = 0; i < surfaceFormats.size(); i++) {
			const auto& format = surfaceFormats[i];
			logger->BeginSection("#" + std::to_string(i) + ":");
			logger->Log("Format: " + vk::to_string(format.format));
			logger->Log("Color Space: " + vk::to_string(format.colorSpace));
			logger->EndSection();

			if (format.format == desiredFormat && format.colorSpace == desiredColorSpace) {
				formatFound = true;
				break;
			}
		}
		logger->EndSection();
		if (!formatFound) {
			std::stringstream ss;
			ss << "Unable to find desired format: " << vk::to_string(desiredFormat)
				<< " and color space: " << vk::to_string(desiredColorSpace);
			SIS_THROW(ss.str());
		}
		colorFormat = desiredFormat;
		colorSpace = desiredColorSpace;
	}

	void RendererImpl::InitSwapchain()
	{
		SIS_DEBUGASSERT(physicalDevice);
		SIS_DEBUGASSERT(surface);
		SIS_DEBUGASSERT(colorFormat);
		SIS_DEBUGASSERT(colorSpace);

		constexpr int desiredMinImageCount = 3; // triple buffering
		auto surfaceCapabilites = physicalDevice.getSurfaceCapabilitiesKHR(*surface);
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
		auto supportedPresentModes = physicalDevice.getSurfacePresentModesKHR(surface.get());
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

		vk::SwapchainCreateInfoKHR swapchainCreateInfo(
			{},
			*surface,
			desiredMinImageCount,
			colorFormat.value(),
			colorSpace.value(),
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
		SIS_DEBUGASSERT(colorFormat);

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
				colorFormat.value(),
				componentMapping,
				subresourceRange
			);
			imageViews.emplace_back(device->createImageViewUnique(imageViewCreateInfo));
		}
	}

	void RendererImpl::InitDepthBuffer()
	{
		SIS_DEBUGASSERT(physicalDevice);
		SIS_DEBUGASSERT(device);
		SIS_DEBUGASSERT(windowExtent);

		DepthBuffer::CreateInfo createInfo{
			vk::Extent2D{windowExtent->width, windowExtent->height},
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
		SIS_DEBUGASSERT(physicalDevice);
		SIS_DEBUGASSERT(descriptorSet);

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
		SIS_DEBUGASSERT(colorFormat);

		vk::AttachmentDescription attachmentDescriptions[2];

		// color
		attachmentDescriptions[0] = vk::AttachmentDescription(
			{},
			colorFormat.value(),
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
		SIS_DEBUGASSERT(windowExtent);
		vk::ImageView attachments[2];
		attachments[1] = depthBuffer->GetImageView();

		for (const auto& imageView : imageViews) {
			attachments[0] = *imageView;
			vk::FramebufferCreateInfo framebufferCreateInfo{
				{},
				*renderPass,
				2,
				attachments,
				windowExtent->width,
				windowExtent->height,
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
		SIS_DEBUGASSERT(physicalDevice);
		auto surfaceCapabilities = physicalDevice.getSurfaceCapabilitiesKHR(*surface);
		bool surfaceChanged = surfaceCapabilities.currentExtent != *windowExtent;
		if (surfaceChanged) {
			logger->Log(
				"Surface extent changed from " + ToString(GetExtent2D(*windowExtent)) +
				" to " + ToString(surfaceCapabilities.currentExtent));
			__debugbreak();
		}
	}

	void RendererImpl::InitVertexBuffer(size_t size)
	{
		logger->BeginSection("Vertex Buffer");
		SIS_DEBUGASSERT(device);
		SIS_DEBUGASSERT(physicalDevice);

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