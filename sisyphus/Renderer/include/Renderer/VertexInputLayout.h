#pragma once
#include <vector>
#include <utility>

namespace Sisyphus::Rendering {

	enum class ComponentType {
		Float,
		UInt,
		SInt
	};

	struct VertexInputAttribute {
		std::vector<std::pair<ComponentType, int>> components;
		uint32_t offset;
	};

	struct VertexInputBinding {
		std::vector<VertexInputAttribute> attributes;
		uint32_t stride;
	};

	struct VertexInputLayout {
		std::vector<VertexInputBinding> bindings;
	};
}