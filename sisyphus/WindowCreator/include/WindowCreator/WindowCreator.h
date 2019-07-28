#pragma once
#include <memory>
#include "Window.h"

namespace WindowCreator {

	enum class Platform {
		Windows
	};

	struct CreateInfo {
		Platform platform;
		int width;
		int height;
	};

	class WindowCreator {
	public:
		std::unique_ptr<Window> Create(CreateInfo createInfo);
	};
}