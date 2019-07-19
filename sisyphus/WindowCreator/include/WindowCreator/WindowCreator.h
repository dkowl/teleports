#pragma once
#include <memory>
#include "Window.h"

namespace WindowCreator {

	enum class Platform {
		Windows
	};

	class WindowCreator {
	public:
		std::unique_ptr<Window> CreateWindow(Platform platform);
	};
}