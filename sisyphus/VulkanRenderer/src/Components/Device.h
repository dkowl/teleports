#pragma once
#include "VulkanComponent.h"
#include "Vulkan.h"

namespace Sisyphus::Rendering::Vulkan {

	class Device : public IVulkanComponent<vk::Device> {
	public:
		void Initialize(const ECS::Entity& inEntity) override;
		static uuids::uuid TypeId();
		static std::string ClassName();
		static ECS::ComponentReferences Dependencies();

		vk::Device GetVulkanObject() const override;

	private:
		vk::UniqueDevice device;
	};
}
