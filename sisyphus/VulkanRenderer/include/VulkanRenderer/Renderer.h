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

		struct UniformBufferData {
			char r;
			char g;
			char b;
		};

		Renderer(CreateInfo ci);
		~Renderer(); // default

		void UpdateUniformBuffer(UniformBufferData data);

	private:
		std::unique_ptr<RendererImpl> impl;
	};
}