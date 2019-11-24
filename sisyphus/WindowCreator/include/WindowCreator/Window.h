#pragma once
#include "WindowEvent.h"
#include <vulkan\vulkan.hpp>
#include <optional>

namespace WindowCreator {

	class Window {

	public:

		virtual ~Window() = default;
		virtual std::optional<WindowEvent> GetEvent() = 0;
		virtual vk::UniqueSurfaceKHR GetVulkanSurface(vk::Instance instance) = 0;
	};
}