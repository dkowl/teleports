#pragma once
#include <Windows.h>
#include "Window.h"

class WindowsWindow : public Window {

public:
	WindowsWindow();
	~WindowsWindow();

	void HandleEvent(WindowEvent event);
	vk::SurfaceKHR GetVulkanSurface();

private:
	HWND window;
};