#pragma once
#include "Renderer/VertexInputLayout.h"
#include "Renderer/ShaderType.h"
#include <string>
#include "uuid.h"

namespace Sisyphus::Rendering {
	struct ShaderInfo {
		uuids::uuid id;
		std::string code;
		ShaderType type;
		std::optional<VertexInputLayout> vertexInputLayout;
	};
}