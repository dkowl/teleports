#pragma once
#include <memory>
#include "ShaderType.h"
#include "uuid.h"

class ILogger;

namespace Vulkan {
	class RendererImpl;

	class Renderer {
	public:
		struct CreateInfo {
			uint32_t windowWidth;
			uint32_t windowHeight;
			ILogger* logger;
		};

		struct UniformBufferData {
			char r;
			char g;
			char b;
		};

		Renderer(CreateInfo ci);
		~Renderer(); // default

		void UpdateUniformBuffer(UniformBufferData data);

		void CreateShader(uuids::uuid id, const std::string& code, ShaderType type);

	private:
		std::unique_ptr<RendererImpl> impl;
	};
}