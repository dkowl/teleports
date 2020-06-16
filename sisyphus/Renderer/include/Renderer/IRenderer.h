#pragma once
#include "Renderer/IDrawable.h"
#include "Renderer/ShaderInfo.h"

namespace Sisyphus::WindowCreator {
	class Window;
}

namespace Sisyphus::Rendering {
	enum class RendererType {
		Vulkan
	};

	struct RendererCreateInfo {
		RendererType type;
		WindowCreator::Window* window;
		std::vector<ShaderInfo> shaders;
	};

	class IRenderer {
	public:
		virtual ~IRenderer() = default;
		virtual void Draw(const IDrawable& drawable) = 0;
	};
}