#include <vector>
#include <string>
#include <iostream>
#include "VulkanRenderer.h"
#include "WindowCreator/WindowCreator.h"
#include "Utils\BreakAssert.h"

namespace wc = WindowCreator;

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

VulkanRenderer::VulkanRenderer():
	instance(nullptr),
	window(nullptr),
	surface(nullptr),
	physicalDevice(nullptr),
	queueFamilyIndex(std::nullopt),
	device(nullptr),
	commandPool(nullptr),
	swapchain(nullptr)
{
	InitInstance();
	InitWindow();
	InitSurface();
	InitPhysicalDevice();
	InitQueueFamilyIndex();
	InitDevice();
	InitCommandPool();
	InitCommandBuffers();
	InitSwapchain();
}

VulkanRenderer::~VulkanRenderer() = default;

void VulkanRenderer::InitInstance()
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

void VulkanRenderer::InitWindow()
{
	wc::WindowCreator windowCreator;
	window = windowCreator.Create(wc::Platform::Windows);
}

void VulkanRenderer::InitSurface()
{
	BreakAssert(instance);
	BreakAssert(window);
	surface = window->GetVulkanSurface(instance.get());
}

void VulkanRenderer::InitPhysicalDevice()
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

void VulkanRenderer::InitQueueFamilyIndex()
{
	BreakAssert(physicalDevice);
	BreakAssert(surface);
	queueFamilyIndex = FindGraphicsQueueFamilyIndex(physicalDevice, *surface);
	if (queueFamilyIndex == -1) {
		throw std::runtime_error("Graphics queue not found in the device");
	}
}

void VulkanRenderer::InitDevice()
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

void VulkanRenderer::InitCommandPool()
{
	BreakAssert(queueFamilyIndex);
	BreakAssert(device);

	vk::CommandPoolCreateInfo commandPoolCreateInfo(
		{},
		queueFamilyIndex.value()
	);
	commandPool = device->createCommandPoolUnique(commandPoolCreateInfo);
}

void VulkanRenderer::InitCommandBuffers()
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

void VulkanRenderer::InitSwapchain()
{
	/*
	vk::SwapchainCreateInfoKHR swapchainCreateInfo(
		{},
		surface,

		);*/
}
