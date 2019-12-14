#pragma once
#include "Component.h"
#include "Vulkan.h"

namespace Sisyphus::Rendering::Vulkan {

	class PhysicalDevice : public IVulkanComponent<vk::PhysicalDevice> {
	public:
		void Initialize(const ComponentManager& componentManager) override;
		static uuids::uuid TypeId();
		static std::string ClassName();
		static Dependencies Dependencies();

		vk::PhysicalDevice GetVulkanObject() const override;

		void Inspect() const;

	private:
		vk::PhysicalDevice physicalDevice;
	};
}