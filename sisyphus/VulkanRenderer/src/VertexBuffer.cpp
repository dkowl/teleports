#include "Pch_VulkanRenderer.h"
#include "VertexBuffer.h"
#include "Utils\Logger.h"
#include "MemoryUtils.h"

namespace Sisyphus::Rendering::Vulkan {
	VertexBuffer::VertexBuffer(CreateInfo inCi) :
		ci(inCi)
	{
		CreateBuffer();
		Logger::Get().Log("Buffer created!");
		AllocateMemory();
		Logger::Get().Log("Memory allocated!");
		BindMemory();
		Logger::Get().Log("Memory bound!");
	}

	DeviceData VertexBuffer::GetDeviceData()
	{
		return DeviceData(ci.device, *memory, ci.sizeInBytes);
	}

	vk::Buffer VertexBuffer::GetBuffer() const
	{
		return *buffer;
	}

	void VertexBuffer::CreateBuffer()
	{
		vk::BufferCreateInfo bufferCreateInfo(
			{},
			ci.sizeInBytes,
			vk::BufferUsageFlagBits::eVertexBuffer
		);
		buffer = ci.device.createBufferUnique(bufferCreateInfo);
	}

	void VertexBuffer::AllocateMemory()
	{
		auto memoryRequirements = ci.device.getBufferMemoryRequirements(*buffer);
		auto memoryTypeIndex = FindMemoryType(
			ci.physicalDevice.getMemoryProperties(),
			memoryRequirements.memoryTypeBits,
			vk::MemoryPropertyFlagBits::eHostVisible | vk::MemoryPropertyFlagBits::eHostCoherent
		);

		memory = ci.device.allocateMemoryUnique(vk::MemoryAllocateInfo(memoryRequirements.size, memoryTypeIndex));
	}
	void VertexBuffer::BindMemory()
	{
		SIS_DEBUGASSERT(buffer);
		SIS_DEBUGASSERT(memory);

		ci.device.bindBufferMemory(*buffer, *memory, 0);
	}
}
