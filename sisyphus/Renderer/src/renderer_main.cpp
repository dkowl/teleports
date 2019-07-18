#include <vulkan/vulkan.hpp>
#include <exception>
#include <iostream>


void InspectDevice(const vk::PhysicalDevice& physicalDevice) {
	auto queueFamilyProperties = physicalDevice.getQueueFamilyProperties();
	std::cout << "Queue Families:\n";
	int index = 1;
	for (auto&& props : queueFamilyProperties) {
		auto flags = props.queueFlags;
		std::cout << "\t#" << index << ": " << vk::to_string(flags) << " Count: " << props.queueCount << std::endl;
		index++;
	}
}

int FindGraphicsQueueFamilyIndex(const vk::PhysicalDevice& physicalDevice) {
	auto queueFamilyProperties = physicalDevice.getQueueFamilyProperties();
	for (int i = 0; i < queueFamilyProperties.size(); i++) {
		if (queueFamilyProperties[i].queueFlags & vk::QueueFlagBits::eGraphics) {
			return i;
		}
	}
	return -1;
}

int main() {
	try {
		vk::ApplicationInfo applicationInfo(
			"Vulkan App",
			VK_MAKE_VERSION(1, 0, 0),
			"Sisyphus",
			VK_MAKE_VERSION(1, 0, 0),
			VK_API_VERSION_1_0
		);

		vk::InstanceCreateInfo instanceCreateInfo(
			{},
			&applicationInfo
		);

		auto instance = vk::createInstanceUnique(instanceCreateInfo);

		auto physicalDevices = instance->enumeratePhysicalDevices();
		if (physicalDevices.empty()) {
			throw::std::runtime_error("No physical devices supporting Vulkan");
		}

		auto& chosenDevice = physicalDevices[0];
		std::cout << "Creating a Vulkan Device from " << chosenDevice.getProperties().deviceName << std::endl;
		InspectDevice(chosenDevice);

		int queueFamilyIndex = FindGraphicsQueueFamilyIndex(chosenDevice);
		if (queueFamilyIndex == -1) {
			throw std::runtime_error("Graphics queue not found in the device");
		}
		vk::DeviceQueueCreateInfo deviceQueueCreateInfo(
			{},
			queueFamilyIndex,
			1
		);

		vk::DeviceCreateInfo deviceCreateInfo(
			{},
			1,
			&deviceQueueCreateInfo
		);

		auto device = chosenDevice.createDeviceUnique(deviceCreateInfo);
	}
	catch (std::exception& e) {
		std::cout << e.what() << std::endl;
	}
}