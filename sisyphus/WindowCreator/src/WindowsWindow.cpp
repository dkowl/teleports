#include "WindowsWindow.h"

WindowsWindow::WindowsWindow()
{
	auto hInstance = GetModuleHandle(nullptr);

	WNDCLASSEX winClass;
	winClass.cbSize = sizeof(WNDCLASSEX);
	winClass.style = CS_HREDRAW | CS_VREDRAW;
	winClass.lpfnWndProc = DefWindowProc;
	winClass.cbClsExtra = 0;
	winClass.cbWndExtra = 0;
	winClass.hInstance = hInstance;
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
	window = CreateWindowEx(
		0,
		"Class Name",
		"Window Name",
		WS_OVERLAPPEDWINDOW | WS_VISIBLE,
		100, 100,
		windowRect.right - windowRect.left,
		windowRect.bottom - windowRect.top,
		nullptr,
		nullptr,
		hInstance,
		nullptr
	);

	if (!window) {
		throw std::runtime_error("Window creation failed!");
	}
}

WindowsWindow::~WindowsWindow()
{
	DestroyWindow(window);
}

void WindowsWindow::HandleEvent(WindowEvent event)
{
}

vk::SurfaceKHR WindowsWindow::GetVulkanSurface()
{
	return vk::SurfaceKHR();
}
