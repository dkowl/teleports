#pragma once
#include "Vulkan.h"
#include "Utils/BreakAssert.h"
#include "Utils/ILogger.h"
#include "DeviceData.h"

namespace Vulkan {

	class UniformBuffer {
	public:
		struct CreateInfo {
			size_t sizeInBytes;
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

			auto deviceData = GetDeviceData();
			deviceData.Set(reinterpret_cast<std::byte*>(&data));
		}

		template<typename T>
		T GetData() {
			BreakAssert(sizeof(T) == ci.sizeInBytes);

			T result;
			auto deviceData = GetDeviceData();
			deviceData.Get(reinterpret_cast<std::byte*>(&result));
			return result;
		}

		DeviceData GetDeviceData();

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
