#pragma once
#include "Vulkan.h"
#include "ECS/Component.h"

namespace Sisyphus::Rendering::Vulkan {
	class DepthBuffer : public ECS::IComponent {
	public:
		static constexpr vk::Format format = vk::Format::eD16Unorm;

		void Initialize() override;
		void RegisterEventHandlers() override;
		static uuids::uuid TypeId();
		static std::string ClassName();
		static ECS::ComponentReferences Dependencies();

		void Clean();
		void Resize();
		
		vk::ImageView GetImageView() const;

	private:
		static vk::UniqueImage CreateImage(vk::Extent2D extent, vk::Device device, vk::PhysicalDevice physicalDevice);
		static vk::UniqueImageView CreateImageView(vk::Image image, vk::Device device);

		vk::UniqueImage image;
		vk::UniqueDeviceMemory memory;
		vk::UniqueImageView imageView;
	};
}