#include "Pch_WindowCreator.h"
#include "WindowCreator.h"
#include "Window.Windows.h"

namespace Sisyphus::WindowCreator {
	std::unique_ptr<Window> WindowCreator::Create(const WindowCreateInfo& ci)
	{
		switch (ci.platform) {
		case Platform::Windows:
			return std::make_unique<WindowsWindow>(WindowsWindow::CreateInfo{ ci.extent });
		default:
			throw std::runtime_error("Invalid window type");
		}
	}
}
