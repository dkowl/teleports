#include "Pch_VulkanRenderer.h"
#include "PhysicalDevice.h"
#include "Instance.h"
#include "Surface.h"
#include "ECS/Entity.h"
#include "Utils/Logger.h"
#include "Utils/DebugMacros.h"

namespace Sisyphus::Rendering::Vulkan {

	SIS_DEFINE_ID(ComponentID_PhysicalDevice, "8ffebfd2bd1b4ac6a35aa814d230e234");

	PhysicalDevice::~PhysicalDevice()
	{
		SIS_DEBUG_ONLY(Logger::Get().Log("~PhysicalDevice"));
	}

	void PhysicalDevice::Initialize(const ECS::Entity& inEntity)
	{
		entity = &inEntity;
		auto physicalDevices = entity->GetComponent<Instance>().GetVulkanObject().enumeratePhysicalDevices();
		if (physicalDevices.empty()) {
			SIS_THROW("No physical devices supporting Vulkan");
		}

		physicalDevice = physicalDevices[0];
		Logger::Get().Log("Creating a Vulkan Device from " + std::string(physicalDevice.getProperties().deviceName));
		Inspect();

		FindGraphicsQueueFamilyIndex();
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
	void PhysicalDevice::HandleEvent(ECS::Events::Initialization, const uuids::uuid& compTypeId)
	{
		if (compTypeId == Surface::TypeId()) {
			auto& surface = entity->GetComponent<Surface>();
			FindPresentQueueFamilyIndex(surface);
		}
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
		auto& logger = Logger::Get();
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
		Logger::Get().LogArgs("Graphics queue family index found: ", graphicsQueueFamilyIndex.value());
	}
	void PhysicalDevice::FindPresentQueueFamilyIndex(vk::SurfaceKHR surface)
	{
		auto queueFamilyProperties = physicalDevice.getQueueFamilyProperties();
		for (int i = 0; i < queueFamilyProperties.size(); i++) {
			if (physicalDevice.getSurfaceSupportKHR(i, surface)) {
				presentQueueFamilyIndex = i;
			}
		}
		Logger::Get().LogArgs("Present queue family index found: ", presentQueueFamilyIndex.value());
	}
	ECS::ComponentReferences PhysicalDevice::WatchList(ECS::Events::Initialization)
	{
		return { {Surface::TypeId()} };
	}
}