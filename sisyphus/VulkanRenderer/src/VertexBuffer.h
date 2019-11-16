#pragma once
#include "Vulkan.h"
#include "DeviceData.h"

namespace Sisyphus::Rendering::Vulkan {
	class VertexBuffer {
	public:
		struct CreateInfo {
			size_t sizeInBytes;
			vk::Device device;
			vk::PhysicalDevice physicalDevice;
		};

		VertexBuffer(CreateInfo inCi);

		DeviceData GetDeviceData();

	private:
		void CreateBuffer();
		void AllocateMemory();
		void BindMemory();

		CreateInfo ci;

		vk::UniqueBuffer buffer;
		vk::UniqueDeviceMemory memory;
	};
}
