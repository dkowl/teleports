#pragma once
#include "Vulkan.h"
#include <string>
#include <optional>
#include "Utils\Throw.h"
#include "Utils\ILogger.h"
#include "Renderer\IRenderer.h"
#include "WindowCreator/WindowExtent.h"

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
			SIS_THROW(std::string("Unable to find ") + name + " Vulkan function");
		}
		outPfn = reinterpret_cast<T>(pfn);
	}

	std::optional<int> FindGraphicsQueueFamilyIndex(vk::PhysicalDevice& physicalDevice, vk::SurfaceKHR& surface);

	vk::Extent2D GetExtent2D(WindowCreator::WindowExtent windowExtent);
	bool operator==(const vk::Extent2D& ex1, const WindowCreator::WindowExtent& ex2);
	std::string ToString(const vk::Extent2D& extent);
}