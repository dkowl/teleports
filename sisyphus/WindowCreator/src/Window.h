#pragma once
#include "WindowEvent.h"
#include <vulkan\vulkan.hpp>

class Window {

public:
	virtual ~Window() = default;
	virtual void HandleEvent(WindowEvent event) = 0;
	virtual vk::SurfaceKHR GetVulkanSurface() = 0;
};