#pragma once
#include "WindowEvent.h"
#include <vulkan\vulkan.hpp>
#include <optional>

namespace Sisyphus::WindowCreator {

	struct WindowExtent {
		uint32_t width;
		uint32_t height;
	};

	class Window {

	public:

		virtual ~Window() = default;
		virtual WindowExtent GetExtent() const = 0;
		virtual std::optional<WindowEvent> GetEvent() = 0;
		virtual vk::UniqueSurfaceKHR GetVulkanSurface(vk::Instance instance) = 0;
	};
}