#include "UniformBuffer.h"
#include <iostream>

Vulkan::UniformBuffer::UniformBuffer(CreateInfo ci):
	ci(ci)
{
	std::cout << "Uniform Buffer:\n";
	CreateBuffer();
	std::cout << "\tBuffer created!\n";
}

Vulkan::UniformBuffer::~UniformBuffer() = default;

void Vulkan::UniformBuffer::CreateBuffer()
{
	vk::BufferCreateInfo bufferCreateInfo(
		{},
		ci.size,
		vk::BufferUsageFlagBits::eUniformBuffer
	);
	buffer = ci.device.createBufferUnique(bufferCreateInfo);
}
