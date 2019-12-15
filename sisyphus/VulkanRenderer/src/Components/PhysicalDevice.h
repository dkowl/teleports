#pragma once
#include "Component.h"
#include "Vulkan.h"

namespace Sisyphus::Rendering::Vulkan {

	class PhysicalDevice : public IVulkanComponent<vk::PhysicalDevice> {
	public:
		void Initialize(const ComponentManager& inComponentManager) override;
		static uuids::uuid TypeId();
		static std::string ClassName();
		static ComponentReferences Dependencies();

		vk::PhysicalDevice GetVulkanObject() const override;
		void HandleEvent(ComponentEvents::Initialization, const uuids::uuid& compTypeId) override;
		static ComponentReferences WatchList(ComponentEvents::Initialization);

		uint32_t GetGraphicsQueueFamilyIndex() const;
		uint32_t GetPresentQueueFamilyIndex() const;
		std::vector<vk::DeviceQueueCreateInfo> GetDeviceQueueCreateInfos() const;

		void Inspect() const;
	private:
		void FindGraphicsQueueFamilyIndex();
		void FindPresentQueueFamilyIndex(vk::SurfaceKHR surface);

		const ComponentManager* componentManager;
		vk::PhysicalDevice physicalDevice;
		std::optional<uint32_t> graphicsQueueFamilyIndex;
		std::optional<uint32_t> presentQueueFamilyIndex;
	};
}