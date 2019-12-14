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
		static Dependencies Dependencies();

		vk::SurfaceKHR GetVulkanObject() const override;

		vk::Extent2D GetExtent() const;

	private:
		WindowCreator::Window* window;
		vk::UniqueSurfaceKHR surface;
	};
}