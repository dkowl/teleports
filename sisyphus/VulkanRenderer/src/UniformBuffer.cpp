#include "UniformBuffer.h"
#include "MemoryUtils.h"
#include "Utils/Throw.h"

namespace Sisyphus::Rendering::Vulkan {
	UniformBuffer::UniformBuffer(CreateInfo ci) :
		ci(ci)
	{
		if (ci.logger == nullptr) {
			SIS_THROW("Logger cannot be null");
		}
		CreateBuffer();
		ci.logger->Log("Buffer created!");
		AllocateMemory();
		ci.logger->Log("Memory allocated!");
		BindMemory();
		ci.logger->Log("Memory bound!");
		UpdateDescriptorSet();
		ci.logger->Log("Descriptor Set updated!");
	}

	UniformBuffer::~UniformBuffer() = default;

	void UniformBuffer::CreateBuffer()
	{
		vk::BufferCreateInfo bufferCreateInfo(
			{},
			ci.sizeInBytes,
			vk::BufferUsageFlagBits::eUniformBuffer
		);
		buffer = ci.device.createBufferUnique(bufferCreateInfo);
	}

	void UniformBuffer::AllocateMemory()
	{
		auto memoryRequirements = ci.device.getBufferMemoryRequirements(*buffer);
		auto memoryTypeIndex = FindMemoryType(
			ci.physicalDevice.getMemoryProperties(),
			memoryRequirements.memoryTypeBits,
			vk::MemoryPropertyFlagBits::eHostVisible | vk::MemoryPropertyFlagBits::eHostCoherent,
			ci.logger
		);

		memory = ci.device.allocateMemoryUnique(vk::MemoryAllocateInfo(memoryRequirements.size, memoryTypeIndex));
	}

	void UniformBuffer::BindMemory()
	{
		SIS_DEBUGASSERT(buffer);
		SIS_DEBUGASSERT(memory);

		ci.device.bindBufferMemory(*buffer, *memory, 0);
	}

	void UniformBuffer::UpdateDescriptorSet()
	{
		SIS_DEBUGASSERT(buffer);

		vk::DescriptorBufferInfo descriptorBufferInfo(
			*buffer,
			0,
			ci.sizeInBytes
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
	DeviceData UniformBuffer::GetDeviceData()
	{
		return DeviceData(ci.device, *memory, ci.sizeInBytes);
	}
}
