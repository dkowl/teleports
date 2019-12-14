#include "Pch_VulkanRenderer.h"
#include "VulkanUtils.h"
#include "Utils\Logger.h"
#include "WindowCreator\Window.h"

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
	vk::Extent2D GetExtent2D(WindowCreator::WindowExtent windowExtent)
	{
		return vk::Extent2D(windowExtent.width, windowExtent.height);
	}

	bool operator==(const vk::Extent2D& ex1, const WindowCreator::WindowExtent& ex2)
	{
		return ex1.width == ex2.width && ex1.height == ex2.height;
	}
	std::string ToString(const vk::Extent2D& extent)
	{
		return std::string("{") + std::to_string(extent.width) + ", " + std::to_string(extent.height) + "}";
	}
}
