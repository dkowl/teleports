#pragma once
#include "PlatformSpecific.h"
#include <memory>
#include <optional>

namespace WindowCreator {
	class Window;
}


class VulkanRenderer {
public:
	struct CreateInfo {
		uint32_t windowWidth;
		uint32_t windowHeight;
	};

	VulkanRenderer(CreateInfo ci);
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
	void InitSwapchainImages();

	CreateInfo ci;
	vk::UniqueInstance instance;
	std::unique_ptr<WindowCreator::Window> window;
	vk::UniqueSurfaceKHR surface;
	vk::PhysicalDevice physicalDevice;
	std::optional<int> queueFamilyIndex;
	vk::UniqueDevice device;
	vk::UniqueCommandPool commandPool;
	std::vector<vk::CommandBuffer> commandBuffers;
	vk::UniqueSwapchainKHR swapchain;
	std::vector<vk::Image> swapchainImages;
	std::vector<vk::UniqueImageView> imageViews;
};