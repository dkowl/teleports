#pragma once
#include "Vulkan.h"
#include "Utils/BreakAssert.h"
#include "Utils\ILogger.h"

namespace Vulkan {

	class UniformBuffer {
	public:
		struct CreateInfo {
			int sizeInBytes;
			vk::Device device;
			vk::PhysicalDevice physicalDevice;
			vk::DescriptorSet descriptorSet;
			ILogger* logger;
		};

		UniformBuffer(CreateInfo ci);
		~UniformBuffer(); // default

		template<typename T>
		void UpdateData(T data) {
			BreakAssert(sizeof(data) == ci.sizeInBytes);

			std::byte* deviceData = static_cast<std::byte*>(ci.device.mapMemory(*memory, 0, ci.sizeInBytes));
			memcpy(deviceData, &data, ci.sizeInBytes);
			ci.device.unmapMemory(*memory);			
		}

	private:
		void CreateBuffer();
		void AllocateMemory();
		void BindMemory();
		void UpdateDescriptorSet();

		CreateInfo ci;
		vk::UniqueBuffer buffer;
		vk::UniqueDeviceMemory memory;
	};
}
