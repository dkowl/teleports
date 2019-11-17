#include <vector>
#include <string>
#include <iostream>
#include <sstream>
#include "RendererImpl.h"
#include "WindowCreator\WindowCreator.h"
#include "Utils\BreakAssert.h"
#include "Utils\UuidGenerator.h"
#include "Utils\Logger.h"
#include "Utils\Throw.h"
#include "VulkanUtils.h"

namespace wc = WindowCreator;

namespace Sisyphus::Rendering::Vulkan {
#if _DEBUG
	constexpr bool enableValidationLayers = true;
#else
	constexpr bool enableValidationLayers = false;
#endif

	constexpr uint64_t timeout = 100000000; // 100ms

	RendererImpl::RendererImpl(const RendererCreateInfo& ci) :
		ci(ci),
		instance(nullptr),
		debugMessenger(nullptr),
		window(nullptr),
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
			Utils::Throw("Logger not found");
		}

		EnumerateInstanceLayerProperties();

		InitInstance();
		logger->Log("Vulkan Instance initialized!");
		if constexpr (enableValidationLayers) {
			InitDebugMessenger();
			logger->Log("Debug Messenger initialized!");
		}
		InitWindow();
		logger->Log("Window initialized!");
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
		InitCommandBuffers();
		logger->Log("Command Buffers initialized!");
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
		BreakAssert(device);
		BreakAssert(!commandBuffers.empty());
		BreakAssert(!framebuffers.empty());
		BreakAssert(renderPass);
		BreakAssert(descriptorSet);

		InitPipeline(drawable.GetVertexStride());
		BreakAssert(pipeline);

		InitVertexBuffer(drawable.GetVertexBufferSize());
		BreakAssert(vertexBuffer);
		vertexBuffer->GetDeviceData().Set(drawable.GetVertexData());

		vk::UniqueSemaphore imageAcquiredSemaphore = device->createSemaphoreUnique(vk::SemaphoreCreateInfo{});
		auto currentBuffer = device->acquireNextImageKHR(*swapchain, timeout, *imageAcquiredSemaphore, nullptr);

		Utils::ThrowAssert(currentBuffer.result == vk::Result::eSuccess, "Failed to acquire an image buffer!");
		Utils::ThrowAssert(
			currentBuffer.value < framebuffers.size(),
			"Acquired image index (" + std::to_string(currentBuffer.value) +
			") higher that the number of available framebuffers (" + std::to_string(framebuffers.size()) + ")");

		auto& commandBuffer = commandBuffers[0];
		commandBuffer->begin(vk::CommandBufferBeginInfo{});

		vk::ClearValue clearValues[2];
		clearValues[0].color = vk::ClearColorValue(std::array<float, 4>{0.9f, 0.8f, 0.7f, 0.9f});
		clearValues[1].depthStencil = vk::ClearDepthStencilValue(1.0f, 0);
		vk::RenderPassBeginInfo renderPassBeginInfo{
			*renderPass,
			*framebuffers[currentBuffer.value],
			vk::Rect2D(vk::Offset2D(0, 0), GetExtent2D(ci.windowExtent)),
			2,
			clearValues
		};
		commandBuffer->beginRenderPass(renderPassBeginInfo, vk::SubpassContents::eInline);
		commandBuffer->bindPipeline(vk::PipelineBindPoint::eGraphics, *pipeline);
		commandBuffer->bindDescriptorSets(vk::PipelineBindPoint::eGraphics, *pipelineLayout, 0, *descriptorSet, nullptr);
		commandBuffer->bindVertexBuffers(0, vertexBuffer->GetBuffer(), { 0 });
		commandBuffer->setViewport(0, vk::Viewport(
			0.0f, 0.0f, 
			static_cast<float>(ci.windowExtent.width), static_cast<float>(ci.windowExtent.height), 
			0.0f, 1.0f));
		commandBuffer->setScissor(0, vk::Rect2D(vk::Offset2D(0, 0), GetExtent2D(ci.windowExtent)));

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
			BreakAssert(false, "Unexpected Draw result!");
		}
		vk::PresentInfoKHR presentInfo{
			0,
			nullptr,
			1,
			&*swapchain,
			&currentBuffer.value
		};
		presentQueue.presentKHR(presentInfo);
	}

	std::vector<const char*> RendererImpl::GetInstanceLayerNames()
	{
		std::vector<const char*> result;
		if constexpr (!enableValidationLayers) {
			return result;
		}

		result.push_back("VK_LAYER_LUNARG_standard_validation");

		for (auto&& name : result) {
			if (!IsLayerEnabled(name)) {
				Utils::Throw(std::string("Cannot find layer ") + std::string(name));
			}
		}

		return result;
	}

	void RendererImpl::InitInstance()
	{
		vk::ApplicationInfo applicationInfo(
			"Vulkan App",
			VK_MAKE_VERSION(1, 0, 0),
			"Sisyphus",
			VK_MAKE_VERSION(1, 0, 0),
			VK_API_VERSION_1_0
		);

		std::vector<const char*> instanceExtensionNames = PlatformSpecific::GetInstanceExtensionNames();
		instanceExtensionNames.push_back(VK_KHR_SURFACE_EXTENSION_NAME);
		if constexpr (enableValidationLayers) {
			instanceExtensionNames.push_back(VK_EXT_DEBUG_UTILS_EXTENSION_NAME);
		}

		std::vector<const char*> layerNames = GetInstanceLayerNames();

		vk::InstanceCreateInfo instanceCreateInfo(
			{},
			&applicationInfo,
			static_cast<uint32_t>(layerNames.size()),
			layerNames.data(),
			static_cast<uint32_t>(instanceExtensionNames.size()),
			instanceExtensionNames.data()
		);

		instance = vk::createInstanceUnique(instanceCreateInfo);
	}

	void RendererImpl::InitDebugMessenger()
	{
		BreakAssert(instance);
		debugMessenger = std::make_unique<DebugMessenger>(*instance);
	}

	void RendererImpl::InitWindow()
	{
		wc::WindowCreator windowCreator;
		window = windowCreator.Create({ wc::Platform::Windows, ci.windowExtent.width, ci.windowExtent.height });
	}

	void RendererImpl::InitSurface()
	{
		BreakAssert(instance);
		BreakAssert(window);
		surface = window->GetVulkanSurface(instance.get());
	}

	void RendererImpl::InitPhysicalDevice()
	{
		BreakAssert(instance);

		auto physicalDevices = instance->enumeratePhysicalDevices();
		if (physicalDevices.empty()) {
			throw::std::runtime_error("No physical devices supporting Vulkan");
		}

		physicalDevice = physicalDevices[0];
		logger->Log("Creating a Vulkan Device from " + std::string(physicalDevice.getProperties().deviceName));
		InspectDevice(physicalDevice, logger);
	}

	void RendererImpl::InitQueueFamilyIndex()
	{
		BreakAssert(physicalDevice);
		BreakAssert(surface);
		queueFamilyIndex = FindGraphicsQueueFamilyIndex(physicalDevice, *surface);
		if (queueFamilyIndex == -1) {
			Utils::Throw("Graphics queue not found in the device");
		}
		logger->Log("Choosing queue family #" + std::to_string(queueFamilyIndex.value()));
	}

	void RendererImpl::InitDevice()
	{
		BreakAssert(physicalDevice);
		BreakAssert(queueFamilyIndex);

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
		BreakAssert(queueFamilyIndex);
		BreakAssert(device);

		vk::CommandPoolCreateInfo commandPoolCreateInfo(
			{},
			queueFamilyIndex.value()
		);
		commandPool = device->createCommandPoolUnique(commandPoolCreateInfo);
	}

	void RendererImpl::InitCommandBuffers()
	{
		BreakAssert(device);
		BreakAssert(commandPool);

		vk::CommandBufferAllocateInfo commandBufferAllocateInfo(
			*commandPool,
			vk::CommandBufferLevel::ePrimary,
			1
		);
		commandBuffers = device->allocateCommandBuffersUnique(commandBufferAllocateInfo);
	}

	void RendererImpl::InitFormatAndColorSpace()
	{
		BreakAssert(surface);

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
			Utils::Throw(ss.str());
		}
		colorFormat = desiredFormat;
		colorSpace = desiredColorSpace;
	}

	void RendererImpl::InitSwapchain()
	{
		BreakAssert(physicalDevice);
		BreakAssert(surface);
		BreakAssert(colorFormat);
		BreakAssert(colorSpace);

		constexpr int desiredMinImageCount = 3; // triple buffering
		auto surfaceCapabilites = physicalDevice.getSurfaceCapabilitiesKHR(*surface);
		logger->Log("Surface minImageCount: " + std::to_string(surfaceCapabilites.minImageCount));
		logger->Log("Surface maxImageCount: " + std::to_string(surfaceCapabilites.maxImageCount));
		if (
			surfaceCapabilites.minImageCount > desiredMinImageCount ||
			surfaceCapabilites.maxImageCount < desiredMinImageCount)
		{
			Utils::Throw("Surface does not support three image buffers");
		}

		logger->Log("Surface extent: (w: " + std::to_string(surfaceCapabilites.currentExtent.width) +
			", h: " +  std::to_string(surfaceCapabilites.currentExtent.height) + ")");

		if (!(surfaceCapabilites.supportedTransforms & vk::SurfaceTransformFlagBitsKHR::eIdentity)) {
			Utils::Throw("Identity surface transform not supported");
		}

		logger->Log("Supported composite alpha: " + vk::to_string(surfaceCapabilites.supportedCompositeAlpha));
		if (!(surfaceCapabilites.supportedCompositeAlpha & vk::CompositeAlphaFlagBitsKHR::eOpaque)) {
			Utils::Throw("Surface opaque composite alpha mode not supported");
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
			Utils::Throw("Present mode " + vk::to_string(desiredPresentMode) + " not supported by GPU");
		}

		vk::SwapchainCreateInfoKHR swapchainCreateInfo(
			{},
			*surface,
			desiredMinImageCount,
			colorFormat.value(),
			colorSpace.value(),
			vk::Extent2D{ ci.windowExtent.width, ci.windowExtent.height },
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
		BreakAssert(swapchain);
		swapchainImages = device->getSwapchainImagesKHR(*swapchain);
	}

	void RendererImpl::InitImageViews()
	{
		BreakAssert(!swapchainImages.empty());
		BreakAssert(colorFormat);

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
		BreakAssert(physicalDevice);
		BreakAssert(device);

		DepthBuffer::CreateInfo createInfo{
			vk::Extent2D{ci.windowExtent.width, ci.windowExtent.height},
			physicalDevice,
			*device,
			logger
		};

		depthBuffer = std::make_unique<DepthBuffer>(createInfo);
	}

	void RendererImpl::InitDescriptorSetLayout()
	{
		BreakAssert(device);

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
		BreakAssert(descriptorSetLayout);
		BreakAssert(device);

		pipelineLayout = device->createPipelineLayoutUnique(vk::PipelineLayoutCreateInfo(
			{},
			1,
			&(*descriptorSetLayout)
		));		
	}

	void RendererImpl::InitDescriptorPool()
	{
		BreakAssert(device);

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
		BreakAssert(device);
		BreakAssert(descriptorPool);

		vk::DescriptorSetAllocateInfo allocateInfo(
			*descriptorPool,
			1,
			&*descriptorSetLayout
		);

		descriptorSet = std::move(device->allocateDescriptorSetsUnique(allocateInfo).front());
	}

	void RendererImpl::InitUniformBuffer()
	{
		BreakAssert(device);
		BreakAssert(physicalDevice);
		BreakAssert(descriptorSet);

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
		BreakAssert(device);
		BreakAssert(colorFormat);

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
		BreakAssert(!imageViews.empty());
		BreakAssert(depthBuffer);
		BreakAssert(device);
		vk::ImageView attachments[2];
		attachments[1] = depthBuffer->GetImageView();

		for (const auto& imageView : imageViews) {
			attachments[0] = *imageView;
			vk::FramebufferCreateInfo framebufferCreateInfo{
				{},
				*renderPass,
				2,
				attachments,
				ci.windowExtent.width,
				ci.windowExtent.height,
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
			Utils::Throw("Unable to find a vertex shader");
		}
		if (fragmentShaderId.is_nil()) {
			Utils::Throw("Unable to find a fragment shader");
		}
	}

	void RendererImpl::InitVertexBuffer(size_t size)
	{
		logger->BeginSection("Vertex Buffer");
		BreakAssert(device);
		BreakAssert(physicalDevice);

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
		BreakAssert(pipelineLayout);
		BreakAssert(renderPass);
		BreakAssert(device);

		if (!ShaderExists(vertexShaderId)) {
			Utils::Throw("Vertex shader not found");
		}
		if (!ShaderExists(fragmentShaderId)) {
			Utils::Throw("Fragment shader not found");
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
			Utils::Throw("Shader " + uuids::to_string(id) + " not found");
		}
		return *shaders[id];
	}

	void RendererImpl::CreateShader(const ShaderInfo& shaderInfo)
	{
		BreakAssert(device);

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
			Utils::Throw("Shader " + uuids::to_string(id) + " does not exist");
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
			BreakAssert(false);
			break;
		}

		logger->Log("Shader " + uuids::to_string(id) + " enabled!");
	}

	void RendererImpl::UpdateUniformBuffer(Renderer::UniformBufferData data)
	{
		BreakAssert(uniformBuffer);
		uniformBuffer->UpdateData(data);
	}

	Renderer::UniformBufferData RendererImpl::GetUniformBufferData()
	{
		BreakAssert(uniformBuffer);
		return uniformBuffer->GetData<Renderer::UniformBufferData>();
	}

}