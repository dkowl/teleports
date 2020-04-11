#include "Pch_VulkanRenderer.h"
#include "Device.h"
#include "PhysicalDevice.h"
#include "Surface.h"
#include "ECS/Entity.h"
#include "MemoryUtils.h"

namespace Sisyphus::Rendering::Vulkan {
	SIS_DEFINE_ID(ComponentID_Device, "0eda2bdf788b42e1ac84a372489562b6");

	void Device::Initialize()
	{
		const auto& physicalDevice = Parent().GetComponent<PhysicalDevice>();
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

		graphicsQueue = device->getQueue(physicalDevice.GetGraphicsQueueFamilyIndex(), 0);
		presentQueue = device->getQueue(physicalDevice.GetPresentQueueFamilyIndex(), 0);

		vk::CommandPoolCreateInfo commandPoolCreateInfo(
			{},
			physicalDevice.GetGraphicsQueueFamilyIndex()
		);
		commandPool = device->createCommandPoolUnique(commandPoolCreateInfo);
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
	vk::Queue Device::GetGraphicsQueue() const
	{
		return graphicsQueue;
	}
	vk::Queue Device::GetPresentQueue() const
	{
		return presentQueue;
	}
	void Device::InitCommandBuffers()
	{
		vk::CommandBufferAllocateInfo commandBufferAllocateInfo(
			*commandPool,
			vk::CommandBufferLevel::ePrimary,
			1
		);
		commandBuffers = device->allocateCommandBuffersUnique(commandBufferAllocateInfo);
	}
	void Device::ResetCommandPool()
	{
		device->resetCommandPool(*commandPool, {});
		commandBuffers.clear();
	}
	vk::CommandBuffer Device::GetCommandBuffer() const
	{
		SIS_THROWASSERT(!commandBuffers.empty());
		return *(commandBuffers[0]);
	}
	vk::UniqueDeviceMemory Device::AllocateAndBindImageMemory(vk::Image image)
	{
		vk::PhysicalDeviceMemoryProperties memoryProperties = Parent().GetComponent<PhysicalDevice>().GetVulkanObject().getMemoryProperties();

		vk::MemoryRequirements memoryRequirements = device->getImageMemoryRequirements(image);
		uint32_t supportedTypeBits = memoryRequirements.memoryTypeBits;

		auto memoryTypeIndex = FindMemoryType(memoryProperties, supportedTypeBits, vk::MemoryPropertyFlagBits::eDeviceLocal);

		auto& logger = Logger();
		auto memory = device->allocateMemoryUnique(vk::MemoryAllocateInfo(memoryRequirements.size, memoryTypeIndex));
		logger.Log(std::to_string(memoryRequirements.size) + " bytes of GPU memory allocated");
		logger.Log("Alignment: " + std::to_string(memoryRequirements.alignment));

		device->bindImageMemory(image, *memory, 0);

		return memory;

	}
}