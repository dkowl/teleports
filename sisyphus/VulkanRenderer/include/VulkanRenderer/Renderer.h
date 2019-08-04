#pragma once
#include <memory>

namespace Vulkan {
	class RendererImpl;

	class Renderer {
	public:
		struct CreateInfo {
			uint32_t windowWidth;
			uint32_t windowHeight;
		};

		Renderer(CreateInfo ci);
		~Renderer(); // default

	private:
		std::unique_ptr<RendererImpl> impl;
	};
}