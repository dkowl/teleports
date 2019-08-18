#pragma once
#include "Vulkan.h"
#include "Utils/BreakAssert.h"

namespace Vulkan {

	class UniformBuffer {
	public:
		struct CreateInfo {
			int size;
			vk::Device device;
			vk::PhysicalDevice physicalDevice;
			vk::DescriptorSet descriptorSet;
		};

		UniformBuffer(CreateInfo ci);
		~UniformBuffer(); // default

		template<typename T>
		void UpdateData(T data) {
			BreakAssert(sizeof(data) == ci.size);

			vk::DescriptorBufferInfo descriptorBufferInfo(
				*buffer,
				0,
				ci.size
			);

			vk::WriteDescriptorSet writeDescriptorSet(
				ci.descriptorSet,
				0,
				0,
				1,
				vk::DescriptorType::eUniformBuffer,
				nullptr,
				&descriptorBufferInfo,
				nullptr
			);

			ci.device.updateDescriptorSets(writeDescriptorSet, {});
		}

	private:
		void CreateBuffer();
		void AllocateMemory();
		void BindMemory();

		CreateInfo ci;
		vk::UniqueBuffer buffer;
		vk::UniqueDeviceMemory memory;
	};
}
