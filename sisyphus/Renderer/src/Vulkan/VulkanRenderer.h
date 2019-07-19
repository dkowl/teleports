#pragma once
#include "PlatformSpecific.h"
#include <memory>
#include <optional>

namespace WindowCreator {
	class Window;
}


class VulkanRenderer {
public:
	VulkanRenderer();
	~VulkanRenderer(); // default

private:
	void InitInstance();
	void InitWindow();
	void InitSurface();
	void InitPhysicalDevice();
	void InitQueueFamilyIndex();
	void InitDevice();
	void InitCommandPool();
	void InitCommandBuffers();
	void InitSwapchain();


	vk::UniqueInstance instance;
	std::unique_ptr<WindowCreator::Window> window;
	vk::UniqueSurfaceKHR surface;
	vk::PhysicalDevice physicalDevice;
	std::optional<int> queueFamilyIndex;
	vk::UniqueDevice device;
	vk::UniqueCommandPool commandPool;
	std::vector<vk::CommandBuffer> commandBuffers;
	vk::UniqueSwapchainKHR swapchain;
};