#pragma once
#include "Vulkan.h"

namespace Vulkan {
	class DeviceData {
	public:
		DeviceData(vk::Device device, vk::DeviceMemory memory, int sizeInBytes);
		~DeviceData();

		DeviceData(const DeviceData&) = delete;
		DeviceData& operator=(const DeviceData&) = delete;
		DeviceData(DeviceData&&) = default;
		DeviceData& operator=(DeviceData&&) = default;

		void Set(const std::byte* data);
		void Get(std::byte* data) const;

	private:
		vk::Device device;
		vk::DeviceMemory memory;
		int sizeInBytes;

		std::byte* deviceData;
	};
}