#pragma once
#include "WindowExtent.h"
#include "WindowEvent.h"
#include <vulkan\vulkan.hpp>
#include <optional>

namespace Sisyphus::WindowCreator {

	class Window {
	public:
		virtual ~Window() = default;
		virtual WindowExtent GetExtent() const = 0;
		virtual std::optional<WindowEvent> GetEvent() = 0;
		virtual vk::UniqueSurfaceKHR GetVulkanSurface(vk::Instance instance) = 0;
	};
}