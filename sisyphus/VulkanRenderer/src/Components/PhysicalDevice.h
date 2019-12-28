#pragma once
#include "VulkanComponent.h"
#include "Vulkan.h"

namespace Sisyphus::Rendering::Vulkan {

	class PhysicalDevice : public IVulkanComponent<vk::PhysicalDevice> {
	public:
		~PhysicalDevice();

		void Initialize() override;
		void RegisterEventHandlers() override;
		static uuids::uuid TypeId();
		static std::string ClassName();
		static ECS::ComponentReferences Dependencies();

		vk::PhysicalDevice GetVulkanObject() const override;

		uint32_t GetGraphicsQueueFamilyIndex() const;
		uint32_t GetPresentQueueFamilyIndex() const;
		std::vector<vk::DeviceQueueCreateInfo> GetDeviceQueueCreateInfos() const;

		void Inspect() const;
	private:
		void FindGraphicsQueueFamilyIndex();
		void FindPresentQueueFamilyIndex();

		vk::PhysicalDevice physicalDevice;
		std::optional<uint32_t> graphicsQueueFamilyIndex;
		std::optional<uint32_t> presentQueueFamilyIndex;
	};
}