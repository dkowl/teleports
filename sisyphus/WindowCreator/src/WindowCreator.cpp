#include "WindowCreator.h"
#include "WindowsWindow.h"
#include <stdexcept>

namespace WindowCreator {
	std::unique_ptr<Window> WindowCreator::Create(Platform platform)
	{
		switch (platform) {
		case Platform::Windows:
			return std::make_unique<WindowsWindow>();
		default:
			throw std::runtime_error("Invalid window type");
		}
	}
}
