#pragma once
#include "IDrawable.h"
#include "ShaderInfo.h"

namespace Rendering {
	enum class RendererType {
		Vulkan
	};

	struct WindowExtent {
		uint32_t width;
		uint32_t height;
	};

	struct RendererCreateInfo {
		RendererType type;
		WindowExtent windowExtent;
		std::vector<ShaderInfo> shaders;
	};

	class IRenderer {

		virtual void Draw(const IDrawable& drawable) const = 0;
	};
}