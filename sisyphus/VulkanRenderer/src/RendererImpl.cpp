#include <vector>
#include <string>
#include <iostream>
#include <sstream>
#include "RendererImpl.h"
#include "WindowCreator\WindowCreator.h"
#include "Utils\BreakAssert.h"

namespace wc = WindowCreator;

namespace Vulkan {
	void InspectDevice(const vk::PhysicalDevice& physicalDevice, ILogger* logger) {
		auto queueFamilyProperties = physicalDevice.getQueueFamilyProperties();
		logger->BeginSection("Queue Families:");
		int index = 1;
		for (auto&& props : queueFamilyProperties) {
			auto flags = props.queueFlags;
			logger->Log("#" + std::to_string(index) + ": " + vk::to_string(flags) + " Count: " + std::to_string(props.queueCount));
			index++;
		}
		logger->EndSection();
	}

	std::optional<int> FindGraphicsQueueFamilyIndex(vk::PhysicalDevice& physicalDevice, vk::SurfaceKHR& surface) {
		auto queueFamilyProperties = physicalDevice.getQueueFamilyProperties();
		for (int i = 0; i < queueFamilyProperties.size(); i++) {
			if ((queueFamilyProperties[i].queueFlags & vk::QueueFlagBits::eGraphics) &&
				physicalDevice.getSurfaceSupportKHR(i, surface)) {
				return i;
			}
		}
		return std::nullopt;
	}

	RendererImpl::RendererImpl(Renderer::CreateInfo ci) :
		ci(ci),
		instance(nullptr),
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
		logger(ci.logger)
	{
		if (logger == nullptr) {
			throw std::runtime_error("Logger not found");
		}

		InitInstance();
		logger->Log("Vulkan instance initialized!");
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
	}

	RendererImpl::~RendererImpl() = default;

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

		vk::InstanceCreateInfo instanceCreateInfo(
			{},
			&applicationInfo,
			0,
			nullptr,
			static_cast<uint32_t>(instanceExtensionNames.size()),
			instanceExtensionNames.data()
		);

		instance = vk::createInstanceUnique(instanceCreateInfo);
	}

	void RendererImpl::InitWindow()
	{
		wc::WindowCreator windowCreator;
		window = windowCreator.Create({ wc::Platform::Windows, ci.windowWidth, ci.windowHeight });
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
			throw std::runtime_error("Graphics queue not found in the device");
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
		commandBuffers = device->allocateCommandBuffers(commandBufferAllocateInfo);
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
			throw std::runtime_error(ss.str());
		}
		colorFormat = desiredFormat;
		colorSpace = desiredColorSpace;
	}

	void RendererImpl::InitSwapchain()
	{
		BreakAssert(physicalDevice);
		BreakAssert(surface);
		BreakAssert(queueFamilyIndex);
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
			throw std::runtime_error("Surface does not support three image buffers");
		}

		logger->Log("Surface extent: (w: " + std::to_string(surfaceCapabilites.currentExtent.width) +
			", h: " +  std::to_string(surfaceCapabilites.currentExtent.height) + ")");

		if (!(surfaceCapabilites.supportedTransforms & vk::SurfaceTransformFlagBitsKHR::eIdentity)) {
			throw std::runtime_error("Identity surface transform not supported");
		}

		logger->Log("Supported composite alpha: " + vk::to_string(surfaceCapabilites.supportedCompositeAlpha));
		if (!(surfaceCapabilites.supportedCompositeAlpha & vk::CompositeAlphaFlagBitsKHR::eOpaque)) {
			throw std::runtime_error("Surface opaque composite alpha mode not supported");
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
			throw std::runtime_error("Present mode " + vk::to_string(desiredPresentMode) + " not supported by GPU");
		}

		vk::SwapchainCreateInfoKHR swapchainCreateInfo(
			{},
			*surface,
			desiredMinImageCount,
			colorFormat.value(),
			colorSpace.value(),
			vk::Extent2D{ ci.windowWidth, ci.windowHeight },
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
			vk::Extent2D{ci.windowWidth, ci.windowHeight},
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
			{},
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

	void RendererImpl::UpdateUniformBuffer(Renderer::UniformBufferData data)
	{
		BreakAssert(uniformBuffer);

		uniformBuffer->UpdateData(data);
	}

}