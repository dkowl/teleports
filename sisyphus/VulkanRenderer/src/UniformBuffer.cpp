#include "UniformBuffer.h"
#include "MemoryUtils.h"

namespace Vulkan {
	UniformBuffer::UniformBuffer(CreateInfo ci) :
		ci(ci)
	{
		if (ci.logger == nullptr) {
			throw std::runtime_error("Logger cannot be null");
		}
		CreateBuffer();
		ci.logger->Log("Buffer created!");
		AllocateMemory();
		ci.logger->Log("Memory allocated!");
		BindMemory();
		ci.logger->Log("Memory bound!");
	}

	UniformBuffer::~UniformBuffer() = default;

	void UniformBuffer::CreateBuffer()
	{
		vk::BufferCreateInfo bufferCreateInfo(
			{},
			ci.size,
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
		BreakAssert(buffer);
		BreakAssert(memory);

		ci.device.bindBufferMemory(*buffer, *memory, 0);
	}
}
