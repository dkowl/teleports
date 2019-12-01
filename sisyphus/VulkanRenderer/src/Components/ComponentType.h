#pragma once
#include <string>

namespace Sisyphus::Rendering::Vulkan {

	enum class ComponentType {
		Instance
	};

	std::string ToString(ComponentType type);
}