#include "WindowCreator.h"
#include "WindowsWindow.h"
#include <stdexcept>

namespace Sisyphus::WindowCreator {
	std::unique_ptr<Window> WindowCreator::Create(CreateInfo ci)
	{
		switch (ci.platform) {
		case Platform::Windows:
			return std::make_unique<WindowsWindow>(WindowsWindow::CreateInfo{ ci.width, ci.height });
		default:
			throw std::runtime_error("Invalid window type");
		}
	}
}
