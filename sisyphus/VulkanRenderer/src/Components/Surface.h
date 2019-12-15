#pragma once
#include "Component.h"
#include "Vulkan.h"

namespace Sisyphus::WindowCreator {
	class Window;
}

namespace Sisyphus::Rendering::Vulkan {

	class Surface : public IVulkanComponent<vk::SurfaceKHR> {
	public:
		Surface(WindowCreator::Window* inWindow);

		void Initialize(const ComponentManager& componentManager) override;
		static uuids::uuid TypeId();
		static std::string ClassName();
		static ComponentReferences Dependencies();

		vk::SurfaceKHR GetVulkanObject() const override;

		void InitFormatAndColorSpace();
		vk::Extent2D GetExtent() const;
		vk::Format GetFormat() const;
		vk::ColorSpaceKHR GetColorSpace() const;

	private:
		const ComponentManager* componentManager;
		WindowCreator::Window* window;
		vk::UniqueSurfaceKHR surface;
		vk::Format format;
		vk::ColorSpaceKHR colorSpace;
	};
}