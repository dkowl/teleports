#include "Pch_VulkanRenderer.h"
#include "Device.h"
#include "PhysicalDevice.h"
#include "Surface.h"
#include "ECS/Entity.h"

namespace Sisyphus::Rendering::Vulkan {
	SIS_DEFINE_ID(ComponentID_Device, "0eda2bdf788b42e1ac84a372489562b6");

	void Device::Initialize(const ECS::Entity& inEntity)
	{
		const auto& physicalDevice = inEntity.GetComponent<PhysicalDevice>();
		auto deviceQueueCreateInfos = physicalDevice.GetDeviceQueueCreateInfos();

		std::vector<const char*> deviceExtensionNames;
		deviceExtensionNames.push_back(VK_KHR_SWAPCHAIN_EXTENSION_NAME);

		vk::DeviceCreateInfo deviceCreateInfo(
			{},
			static_cast<uint32_t>(deviceQueueCreateInfos.size()),
			deviceQueueCreateInfos.data(),
			0,
			nullptr,
			static_cast<uint32_t>(deviceExtensionNames.size()),
			deviceExtensionNames.data()
		);

		device = physicalDevice.GetVulkanObject().createDeviceUnique(deviceCreateInfo);
	}
	uuids::uuid Device::TypeId()
	{
		return ComponentID_Device;
	}
	std::string Device::ClassName()
	{
		return "Device";
	}
	ECS::ComponentReferences Device::Dependencies()
	{
		return { {PhysicalDevice::TypeId()}, {Surface::TypeId()} };
	}
	vk::Device Device::GetVulkanObject() const
	{
		return *device;
	}
}