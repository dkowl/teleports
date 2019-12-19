#pragma once
#include "ECS/Component.h"
#include "Vulkan.h"

namespace Sisyphus::WindowCreator {
	class Window;
}

namespace Sisyphus::Rendering::Vulkan {

	class Surface : public ECS::IVulkanComponent<vk::SurfaceKHR> {
	public:
		Surface(WindowCreator::Window* inWindow);
		~Surface();

		void Initialize(const ECS::Entity& inEntity) override;
		static uuids::uuid TypeId();
		static std::string ClassName();
		static ECS::ComponentReferences Dependencies();

		vk::SurfaceKHR GetVulkanObject() const override;

		void InitFormatAndColorSpace();
		vk::Extent2D GetExtent() const;
		vk::Format GetFormat() const;
		vk::ColorSpaceKHR GetColorSpace() const;

	private:
		const ECS::Entity* entity;
		WindowCreator::Window* window;
		vk::UniqueSurfaceKHR surface;
		vk::Format format;
		vk::ColorSpaceKHR colorSpace;
	};
}