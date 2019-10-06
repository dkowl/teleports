#include "VulkanUtils.h"
#include "Utils\Logger.h"

namespace Vulkan {
	Version::Version(uint32_t version) :
		major(version >> 22),
		minor((version >> 12) & 0x3ff),
		patch(version & 0xfff)
	{
	}

	std::string Version::ToString(uint32_t version)
	{
		Version v(version);
		return std::to_string(v.major) + "." + std::to_string(v.minor) + "." + std::to_string(v.patch);
	}

	void EnumerateInstanceLayerProperties()
	{
		auto& logger = Logger::Get();
		auto layerProperties = vk::enumerateInstanceLayerProperties();
		logger.BeginSection("Vulkan Instance Layers");
		if (layerProperties.empty()) {
			logger.Log("No layers. Set the environment variable VK_LAYER_PATH to point to the location of your layers");
		}
		for (const auto& lp : layerProperties) {
			logger.BeginSection(lp.layerName);
			logger.Log("Version: " + std::to_string(lp.implementationVersion));
			logger.Log("API Version: " + Version::ToString(lp.specVersion));
			logger.Log("Description: " + std::string(lp.description));
			logger.EndSection();
		}
		logger.EndSection();

	}
}
