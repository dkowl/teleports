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
			uint8_t r;
			uint8_t g;
			uint8_t b;
		};

		struct VertexBufferData {
			struct Vertex {
				float x;
				float y;
				float z;
			};

			Vertex vertices[4];
		};

		Renderer(CreateInfo ci);
		~Renderer(); // default

		void UpdateUniformBuffer(UniformBufferData data);
		UniformBufferData GetUniformBufferData();

		void UpdateVertexBuffer(VertexBufferData data);
		VertexBufferData GetVertexBufferData();

		uuids::uuid CreateShader(const std::string& code, ShaderType type);
		void EnableShader(uuids::uuid id);

	private:
		std::unique_ptr<RendererImpl> impl;
	};
}