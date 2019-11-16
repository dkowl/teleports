#pragma once
#include <memory>
#include <string>
#include <vector>
#include "uuid.h"
#include "IRenderer.h"
#include "ShaderInfo.h"

namespace Rendering {
	class RendererFactory {

		std::unique_ptr<IRenderer> Create(const RendererCreateInfo& ci);
	};
}