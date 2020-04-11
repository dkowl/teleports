#include "WindowCreator.h"
#include "Utils/PlatformMacros.h"
#ifdef SIS_WINDOWS
#include "Window.Windows.h"
#endif

namespace Sisyphus::WindowCreator {
	std::unique_ptr<Window> WindowCreator::Create(const WindowCreateInfo& ci)
	{
#ifdef SIS_WINDOWS
		return std::make_unique<WindowsWindow>(WindowsWindow::CreateInfo{ ci.extent });
#else
		throw std::runtime_error("Invalid window type");
#endif
	}
}
