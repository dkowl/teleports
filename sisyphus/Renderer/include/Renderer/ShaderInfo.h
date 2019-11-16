#pragma once
#include "ShaderType.h"
#include <string>
#include "uuid.h"

namespace Sisyphus::Rendering {
	struct ShaderInfo {
		uuids::uuid id;
		std::string code;
		ShaderType type;
	};
}