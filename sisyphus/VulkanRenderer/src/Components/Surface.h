#pragma once
#include "VulkanComponent.h"
#include "Vulkan.h"

namespace Sisyphus::WindowCreator {
	class Window;
}

namespace Sisyphus::Rendering::Vulkan {

	class Surface : public IVulkanComponent<vk::SurfaceKHR> {
	public:
		Surface(WindowCreator::Window* inWindow);
		~Surface();

		void Initialize() override;
		static uuids::uuid TypeId();
		static std::string ClassName();
		static ECS::ComponentReferences Dependencies();

		vk::SurfaceKHR GetVulkanObject() const override;

		void InitFormatAndColorSpace();
		void DetectResize();
		vk::Extent2D GetExtent() const;
		vk::Format GetFormat() const;
		vk::ColorSpaceKHR GetColorSpace() const;

	private:
		WindowCreator::Window* window;
		vk::UniqueSurfaceKHR surface;
		vk::Format format;
		vk::ColorSpaceKHR colorSpace;
		vk::Extent2D extent;
	};
}