#include "VulkanUtils.h"
#include "Utils\Logger.h"

namespace Sisyphus::Rendering::Vulkan {
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
	bool IsLayerEnabled(std::string layerName)
	{
		auto layerProperties = vk::enumerateInstanceLayerProperties();
		for (const auto& lp : layerProperties) {
			if (layerName == lp.layerName) {
				return true;
			}
		}
		return false;
	}

	void InspectDevice(const vk::PhysicalDevice& physicalDevice, ILogger* logger) {
		auto queueFamilyProperties = physicalDevice.getQueueFamilyProperties();
		logger->BeginSection("Queue Families:");
		int index = 1;
		for (auto&& props : queueFamilyProperties) {
			auto flags = props.queueFlags;
			logger->Log("#" + std::to_string(index) + ": " + vk::to_string(flags) + " Count: " + std::to_string(props.queueCount));
			index++;
		}
		logger->EndSection();
	}

	std::optional<int> FindGraphicsQueueFamilyIndex(vk::PhysicalDevice& physicalDevice, vk::SurfaceKHR& surface) {
		auto queueFamilyProperties = physicalDevice.getQueueFamilyProperties();
		for (int i = 0; i < queueFamilyProperties.size(); i++) {
			if ((queueFamilyProperties[i].queueFlags & vk::QueueFlagBits::eGraphics) &&
				physicalDevice.getSurfaceSupportKHR(i, surface)) {
				return i;
			}
		}
		return std::nullopt;
	}
}
