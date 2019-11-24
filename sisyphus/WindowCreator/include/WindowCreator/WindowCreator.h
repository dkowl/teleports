#pragma once
#include <memory>
#include "Window.h"

namespace Sisyphus::WindowCreator {

	enum class Platform {
		Windows
	};

	struct CreateInfo {
		Platform platform;
		uint32_t width;
		uint32_t height;
	};

	class WindowCreator {
	public:
		std::unique_ptr<Window> Create(CreateInfo createInfo);
	};
}