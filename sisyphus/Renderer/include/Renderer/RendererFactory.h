#pragma once
#include <memory>
#include <string>
#include <vector>
#include "uuid.h"
#include "IRenderer.h"
#include "ShaderInfo.h"

namespace Sisyphus::Rendering {
	class RendererFactory {
	public:
		std::unique_ptr<IRenderer> Create(const RendererCreateInfo& ci);
	};
}