#include "Pch_VulkanRenderer.h"
#include "PhysicalDevice.h"
#include "Instance.h"
#include "Surface.h"
#include "ECS/Entity.h"
#include "Logger/Logger.h"
#include "Utils/DebugMacros.h"

namespace Sisyphus::Rendering::Vulkan {

	SIS_DEFINE_ID(ComponentID_PhysicalDevice, "8ffebfd2bd1b4ac6a35aa814d230e234");

	PhysicalDevice::~PhysicalDevice()
	{
		SIS_DEBUG_ONLY(Logger().Log("~PhysicalDevice"));
	}

	void PhysicalDevice::Initialize()
	{
		auto physicalDevices = Parent().GetComponent<Instance>().GetVulkanObject().enumeratePhysicalDevices();
		if (physicalDevices.empty()) {
			SIS_THROW("No physical devices supporting Vulkan");
		}

		physicalDevice = physicalDevices[0];
		Logger().Log("Creating a Vulkan Device from " + std::string(physicalDevice.getProperties().deviceName));
		Inspect();

		FindGraphicsQueueFamilyIndex();
	}
	void PhysicalDevice::RegisterEventHandlers()
	{
		RegisterEventHandler<ECS::Events::Initialization, Surface>(std::bind(&PhysicalDevice::FindPresentQueueFamilyIndex, this));
	}
	uuids::uuid PhysicalDevice::TypeId()
	{
		return ComponentID_PhysicalDevice;
	}
	std::string PhysicalDevice::ClassName()
	{
		return "PhysicalDevice";
	}
	ECS::ComponentReferences PhysicalDevice::Dependencies()
	{
		return { {Instance::TypeId()} };
	}
	vk::PhysicalDevice PhysicalDevice::GetVulkanObject() const
	{
		return physicalDevice;
	}
	uint32_t PhysicalDevice::GetGraphicsQueueFamilyIndex() const
	{
		SIS_THROWASSERT(graphicsQueueFamilyIndex);
		return graphicsQueueFamilyIndex.value();
	}
	uint32_t PhysicalDevice::GetPresentQueueFamilyIndex() const
	{
		SIS_THROWASSERT(presentQueueFamilyIndex);
		return presentQueueFamilyIndex.value();
	}
	std::vector<vk::DeviceQueueCreateInfo> PhysicalDevice::GetDeviceQueueCreateInfos() const
	{
		std::vector<vk::DeviceQueueCreateInfo> result;
		SIS_THROWASSERT(graphicsQueueFamilyIndex);
		result.emplace_back( vk::DeviceQueueCreateInfo{
			{}, graphicsQueueFamilyIndex.value(), 1
		} );
		SIS_THROWASSERT(presentQueueFamilyIndex);
		if (presentQueueFamilyIndex != graphicsQueueFamilyIndex) {
			result.emplace_back(vk::DeviceQueueCreateInfo{
				{}, presentQueueFamilyIndex.value(), 1
			});
		}
		return result;
	}
	void PhysicalDevice::Inspect() const
	{
		auto& logger = Logger();
		auto queueFamilyProperties = physicalDevice.getQueueFamilyProperties();
		logger.BeginSection("Queue Families:");
		int index = 1;
		for (auto&& props : queueFamilyProperties) {
			auto flags = props.queueFlags;
			logger.Log("#" + std::to_string(index) + ": " + vk::to_string(flags) + " Count: " + std::to_string(props.queueCount));
			index++;
		}
		logger.EndSection();
	}
	void PhysicalDevice::FindGraphicsQueueFamilyIndex()
	{
		auto queueFamilyProperties = physicalDevice.getQueueFamilyProperties();
		for (int i = 0; i < queueFamilyProperties.size(); i++) {
			if (queueFamilyProperties[i].queueFlags & vk::QueueFlagBits::eGraphics) {
				graphicsQueueFamilyIndex = i;
			}
		}
		Logger().LogArgs("Graphics queue family index found: ", graphicsQueueFamilyIndex.value());
	}
	void PhysicalDevice::FindPresentQueueFamilyIndex()
	{
		auto& surface = Parent().GetComponent<Surface>();

		auto queueFamilyProperties = physicalDevice.getQueueFamilyProperties();
		for (int i = 0; i < queueFamilyProperties.size(); i++) {
			if (physicalDevice.getSurfaceSupportKHR(i, surface)) {
				presentQueueFamilyIndex = i;
			}
		}
		Logger().LogArgs("Present queue family index found: ", presentQueueFamilyIndex.value());
	}
}