#pragma once
#include "WindowEvent.h"
#include <vulkan\vulkan.hpp>

namespace WindowCreator {
	class Window {

	public:
		virtual ~Window() = default;
		virtual void HandleEvent(WindowEvent event) = 0;
		virtual vk::SurfaceKHR GetVulkanSurface(vk::Instance instance) = 0;
	};
}