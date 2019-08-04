#pragma once
#include "Vulkan.h"
#include "DepthBuffer.h"
#include "PlatformSpecific.h"
#include "Renderer.h"
#include <memory>
#include <optional>

namespace WindowCreator {
	class Window;
}

namespace Vulkan {

	class RendererImpl {
	public:
		RendererImpl(Renderer::CreateInfo ci);
		~RendererImpl(); // default

	private:
		void InitInstance();
		void InitWindow();
		void InitSurface();
		void InitPhysicalDevice();
		void InitQueueFamilyIndex();
		void InitDevice();
		void InitCommandPool();
		void InitCommandBuffers();
		void InitFormatAndColorSpace();
		void InitSwapchain();
		void InitSwapchainImages();
		void InitImageViews();
		void InitDepthBuffer();

		Renderer::CreateInfo ci;
		vk::UniqueInstance instance;
		std::unique_ptr<WindowCreator::Window> window;
		vk::UniqueSurfaceKHR surface;
		vk::PhysicalDevice physicalDevice;
		std::optional<int> queueFamilyIndex;
		vk::UniqueDevice device;
		vk::UniqueCommandPool commandPool;
		std::vector<vk::CommandBuffer> commandBuffers;
		std::optional<vk::Format> format;
		std::optional<vk::ColorSpaceKHR> colorSpace;
		vk::UniqueSwapchainKHR swapchain;
		std::vector<vk::Image> swapchainImages;
		std::vector<vk::UniqueImageView> imageViews;
		std::unique_ptr<DepthBuffer> depthBuffer;
	};
}