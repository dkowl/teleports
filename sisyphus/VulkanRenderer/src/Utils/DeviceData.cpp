#include "DeviceData.h"

namespace Vulkan {
	DeviceData::DeviceData(vk::Device device, vk::DeviceMemory memory, int sizeInBytes) :
		device(device),
		memory(memory),
		sizeInBytes(sizeInBytes),
		deviceData(static_cast<std::byte*>(device.mapMemory(memory, 0, sizeInBytes)))
	{

	}

	DeviceData::~DeviceData()
	{
		device.unmapMemory(memory);
	}

	void DeviceData::Set(const std::byte* data)
	{
		memcpy(deviceData, data, sizeInBytes);
	}

	void DeviceData::Get(std::byte* data) const
	{
		memcpy(data, deviceData, sizeInBytes);
	}
}