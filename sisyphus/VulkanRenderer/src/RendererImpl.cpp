#include <vector>
#include <string>
#include <iostream>
#include <sstream>
#include "RendererImpl.h"
#include "WindowCreator\WindowCreator.h"
#include "Utils\BreakAssert.h"

namespace wc = WindowCreator;

namespace Vulkan {
	void InspectDevice(const vk::PhysicalDevice& physicalDevice) {
		auto queueFamilyProperties = physicalDevice.getQueueFamilyProperties();
		std::cout << "Queue Families:\n";
		int index = 1;
		for (auto&& props : queueFamilyProperties) {
			auto flags = props.queueFlags;
			std::cout << "\t#" << index << ": " << vk::to_string(flags) << " Count: " << props.queueCount << std::endl;
			index++;
		}
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
		format(std::nullopt),
		colorSpace(std::nullopt),
		swapchain(nullptr),
		depthBuffer(nullptr)
	{
		InitInstance();
		std::cout << "Vulkan instance initialized!\n\n";
		InitWindow();
		std::cout << "Window initialized!\n\n";
		InitSurface();
		std::cout << "Surface initialized!\n\n";
		InitPhysicalDevice();
		std::cout << "Physical Device initialized!\n\n";
		InitQueueFamilyIndex();
		std::cout << "Queue Family Index initialized!\n\n";
		InitDevice();
		std::cout << "Vulkan Device initialized!\n\n";
		InitCommandPool();
		std::cout << "Command Pool initialized!\n\n";
		InitCommandBuffers();
		std::cout << "Command Buffers initialized!\n\n";
		InitFormatAndColorSpace();
		std::cout << "Format initialized: " << vk::to_string(format.value()) << std::endl;
		std::cout << "Color space initialized: " << vk::to_string(colorSpace.value()) << std::endl << std::endl;
		InitSwapchain();
		std::cout << "Swapchain initialized!\n\n";
		InitSwapchainImages();
		std::cout << swapchainImages.size() << " Swapchain Images initialized!\n\n";
		InitImageViews();
		std::cout << imageViews.size() << " Image Views initialized!\n\n";
		InitDepthBuffer();
		std::cout << "Depth Buffer initialized!\n\n";
		InitDescriptorSetLayout();
		std::cout << "Descriptor Set Layout initialized!\n\n";
		InitPipelineLayout();
		std::cout << "Pipeline Layout initialized!\n\n";
		InitDescriptorPool();
		std::cout << "Descriptor Pool initialized!\n\n";
		InitDescriptorSet();
		std::cout << "Descriptor Set initialized!\n\n";
		InitUniformBuffer();
		std::cout << "Uniform Buffer initialized!\n\n";
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
		std::cout << "Creating a Vulkan Device from " << physicalDevice.getProperties().deviceName << std::endl;
		InspectDevice(physicalDevice);
	}

	void RendererImpl::InitQueueFamilyIndex()
	{
		BreakAssert(physicalDevice);
		BreakAssert(surface);
		queueFamilyIndex = FindGraphicsQueueFamilyIndex(physicalDevice, *surface);
		if (queueFamilyIndex == -1) {
			throw std::runtime_error("Graphics queue not found in the device");
		}
		std::cout << "Choosing queue family #" << queueFamilyIndex.value() << std::endl;
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
		std::cout << "Surface formats:\n";
		for (int i = 0; i < surfaceFormats.size(); i++) {
			const auto& format = surfaceFormats[i];
			std::cout << "\t#" << i << ": \n";
			std::cout << "\t\tFormat: " << vk::to_string(format.format) << std::endl;
			std::cout << "\t\tColor Space: " << vk::to_string(format.colorSpace) << std::endl;

			if (format.format == desiredFormat && format.colorSpace == desiredColorSpace) {
				formatFound = true;
				break;
			}
		}
		if (!formatFound) {
			std::stringstream ss;
			ss << "Unable to find desired format: " << vk::to_string(desiredFormat)
				<< " and color space: " << vk::to_string(desiredColorSpace);
			throw std::runtime_error(ss.str());
		}
		format = desiredFormat;
		colorSpace = desiredColorSpace;
	}

	void RendererImpl::InitSwapchain()
	{
		BreakAssert(physicalDevice);
		BreakAssert(surface);
		BreakAssert(queueFamilyIndex);
		BreakAssert(format);
		BreakAssert(colorSpace);

		constexpr int desiredMinImageCount = 3; // triple buffering
		auto surfaceCapabilites = physicalDevice.getSurfaceCapabilitiesKHR(*surface);
		std::cout << "Surface minImageCount: " << surfaceCapabilites.minImageCount << std::endl;
		std::cout << "Surface maxImageCount: " << surfaceCapabilites.maxImageCount << std::endl;
		if (
			surfaceCapabilites.minImageCount > desiredMinImageCount ||
			surfaceCapabilites.maxImageCount < desiredMinImageCount)
		{
			throw std::runtime_error("Surface does not support three image buffers");
		}

		std::cout << "Surface extent: (w: " << surfaceCapabilites.currentExtent.width <<
			", h: " << surfaceCapabilites.currentExtent.height << ")\n";

		if (!(surfaceCapabilites.supportedTransforms & vk::SurfaceTransformFlagBitsKHR::eIdentity)) {
			throw std::runtime_error("Identity surface transform not supported");
		}

		std::cout << "Supported composite alpha: " << vk::to_string(surfaceCapabilites.supportedCompositeAlpha) << "\n";
		if (!(surfaceCapabilites.supportedCompositeAlpha & vk::CompositeAlphaFlagBitsKHR::eOpaque)) {
			throw std::runtime_error("Surface opaque composite alpha mode not supported");
		}

		auto desiredPresentMode = vk::PresentModeKHR::eFifoRelaxed;
		auto supportedPresentModes = physicalDevice.getSurfacePresentModesKHR(surface.get());
		bool modeFound = false;
		std::cout << "Supported present modes: ";
		for (auto&& mode : supportedPresentModes) {
			std::cout << vk::to_string(mode) << "\n";
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
			format.value(),
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
		BreakAssert(format);

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
				format.value(),
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
			*device
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
			*descriptorSet
		};

		uniformBuffer = std::make_unique<UniformBuffer>(createInfo);
	}

	void RendererImpl::UpdateUniformBuffer(Renderer::UniformBufferData data)
	{
		BreakAssert(uniformBuffer);

		uniformBuffer->UpdateData(data);
	}

}