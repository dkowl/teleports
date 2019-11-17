#pragma once
#include <memory>
#include "uuid.h"
#include "Renderer/ShaderType.h"
#include "Renderer/IRenderer.h"
#include "Renderer/ShaderInfo.h"

class ILogger;

namespace Sisyphus::Rendering::Vulkan {
	class RendererImpl;

	class Renderer : public IRenderer {
	public:
		struct UniformBufferData {
			uint8_t r;
			uint8_t g;
			uint8_t b;
		};

		Renderer(const RendererCreateInfo& ci);
		~Renderer(); // default

		void Draw(const IDrawable& drawable) override;

		void UpdateUniformBuffer(UniformBufferData data);
		UniformBufferData GetUniformBufferData();

		void CreateShader(const ShaderInfo& shaderInfo);
		void EnableShader(uuids::uuid id);

	private:
		std::unique_ptr<RendererImpl> impl;
	};
}