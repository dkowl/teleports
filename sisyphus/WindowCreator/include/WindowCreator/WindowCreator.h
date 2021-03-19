#pragma once
#include <memory>
#include "Utils/Platform.h"
#include "Window.h"

namespace Sisyphus::WindowCreator {

	struct WindowCreateInfo {
		Platform platform;
		WindowExtent extent;
	};

	class WindowCreator {
	public:
		std::unique_ptr<Window> Create(const WindowCreateInfo& createInfo);
	};
}