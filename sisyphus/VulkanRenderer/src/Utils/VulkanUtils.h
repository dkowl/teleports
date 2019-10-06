#pragma once
#include "Vulkan.h"
#include <string>

namespace Vulkan {
	struct Version {
		Version(uint32_t inVersion);

		uint32_t major;
		uint32_t minor;
		uint32_t patch;

		static std::string ToString(uint32_t version);
	};

	void EnumerateInstanceLayerProperties();
	bool IsLayerEnabled(std::string layerName);
}