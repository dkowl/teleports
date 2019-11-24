#pragma once
#include <memory>
#include "Window.h"

namespace Sisyphus::WindowCreator {

	enum class Platform {
		Windows
	};

	struct WindowCreateInfo {
		Platform platform;
		WindowExtent extent;
	};

	class WindowCreator {
	public:
		std::unique_ptr<Window> Create(const WindowCreateInfo& createInfo);
	};
}