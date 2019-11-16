#pragma once
#include "Vulkan.h"
#include <string>
#include "Utils\Throw.h"

namespace Sisyphus::Rendering::Vulkan {
	struct Version {
		Version(uint32_t inVersion);

		uint32_t major;
		uint32_t minor;
		uint32_t patch;

		static std::string ToString(uint32_t version);
	};

	void EnumerateInstanceLayerProperties();
	bool IsLayerEnabled(std::string layerName);

	template<typename T>
	void LoadFunction(std::string name, const vk::Instance& instance, T& outPfn) {
		auto pfn = instance.getProcAddr(name);
		if (pfn == nullptr) {
			Utils::Throw(std::string("Unable to find ") + name + " Vulkan function");
		}
		outPfn = reinterpret_cast<T>(pfn);
	}
}