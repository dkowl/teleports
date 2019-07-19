#define VK_USE_PLATFORM_WIN32_KHR
#include "WindowsWindow.h"

namespace WindowCreator {
	struct WindowsWindow::PrivateData {
		HINSTANCE hInstance;
		HWND window;
	};

	WindowsWindow::WindowsWindow() :
		data(std::make_unique<PrivateData>())
	{
		data->hInstance = GetModuleHandle(nullptr);

		WNDCLASSEX winClass;
		winClass.cbSize = sizeof(WNDCLASSEX);
		winClass.style = CS_HREDRAW | CS_VREDRAW;
		winClass.lpfnWndProc = DefWindowProc;
		winClass.cbClsExtra = 0;
		winClass.cbWndExtra = 0;
		winClass.hInstance = data->hInstance;
		winClass.hIcon = LoadIcon(nullptr, IDI_APPLICATION);
		winClass.hCursor = LoadCursor(nullptr, IDC_ARROW);
		winClass.hbrBackground = (HBRUSH)GetStockObject(WHITE_BRUSH);
		winClass.lpszMenuName = nullptr;
		winClass.lpszClassName = "Class Name";
		winClass.hIconSm = LoadIcon(nullptr, IDI_WINLOGO);

		if (!RegisterClassEx(&winClass)) {
			throw std::runtime_error("Failed to register a Windows class");
		}

		RECT windowRect = { 0, 0, 800, 600 };
		AdjustWindowRect(&windowRect, WS_OVERLAPPEDWINDOW, FALSE);
		data->window = CreateWindowEx(
			0,
			"Class Name",
			"Window Name",
			WS_OVERLAPPEDWINDOW | WS_VISIBLE,
			100, 100,
			windowRect.right - windowRect.left,
			windowRect.bottom - windowRect.top,
			nullptr,
			nullptr,
			data->hInstance,
			nullptr
		);

		if (!data->window) {
			throw std::runtime_error("Window creation failed!");
		}
	}

	WindowsWindow::~WindowsWindow()
	{
		DestroyWindow(data->window);
	}

	void WindowsWindow::HandleEvent(WindowEvent event)
	{
	}

	vk::UniqueSurfaceKHR WindowsWindow::GetVulkanSurface(vk::Instance instance)
	{
		vk::Win32SurfaceCreateInfoKHR surfaceCreateInfo(
			{},
			data->hInstance,
			data->window
		);
		return instance.createWin32SurfaceKHRUnique(surfaceCreateInfo);
	}
}
