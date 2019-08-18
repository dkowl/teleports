#include "UniformBuffer.h"
#include "MemoryUtils.h"
#include <iostream>

namespace Vulkan {
	UniformBuffer::UniformBuffer(CreateInfo ci) :
		ci(ci)
	{
		std::cout << "Uniform Buffer:\n";
		CreateBuffer();
		std::cout << "\tBuffer created!\n";
		AllocateMemory();
		std::cout << "\tMemory allocated!\n";
		BindMemory();
		std::cout << "\tMemory bound!\n";
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
			vk::MemoryPropertyFlagBits::eHostVisible | vk::MemoryPropertyFlagBits::eHostCoherent
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
