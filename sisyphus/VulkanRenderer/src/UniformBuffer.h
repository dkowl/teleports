#pragma once
#include "Vulkan.h"

namespace Vulkan {

	class UniformBuffer {
	public:
		struct CreateInfo {
			int size;
			vk::Device device;
			vk::PhysicalDevice physicalDevice;
		};

		UniformBuffer(CreateInfo ci);
		~UniformBuffer(); // default

	private:
		void CreateBuffer();
		void AllocateMemory();

		CreateInfo ci;
		vk::UniqueBuffer buffer;
		vk::UniqueDeviceMemory memory;
	};
}
