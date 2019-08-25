#include "MemoryUtils.h"
#include "Utils\BreakAssert.h"

namespace Vulkan {
	void InspectMemoryProperties(vk::PhysicalDeviceMemoryProperties properties, ILogger* logger) {
		BreakAssert(logger != nullptr);

		logger->BeginSection("Memory types");
		for (uint32_t i = 0; i < properties.memoryTypeCount; i++) {
			auto type = properties.memoryTypes[i];
			logger->BeginSection("#" + std::to_string(i));
			logger->Log("Heap index: " + std::to_string(type.heapIndex));
			logger->Log("Properties: " + vk::to_string(type.propertyFlags));
			logger->EndSection();
		}
		logger->EndSection();

		logger->BeginSection("Heaps:");
		for (uint32_t i = 0; i < properties.memoryHeapCount; i++) {
			auto heap = properties.memoryHeaps[i];
			logger->BeginSection("#" + std::to_string(i));
			logger->Log("Size: " + std::to_string(heap.size));
			logger->Log("Flags: " + vk::to_string(heap.flags));
			logger->EndSection();
		}
		logger->EndSection();
	}

	uint32_t FindMemoryType(
		vk::PhysicalDeviceMemoryProperties properties, 
		uint32_t supportedTypeBits, 
		vk::MemoryPropertyFlags propertyFlags,
		ILogger* logger)
	{
		static bool inspected = false;
		if (!inspected && logger != nullptr) {
			InspectMemoryProperties(properties, logger);
			inspected = true;
		}

		uint32_t memoryTypeIndex = uint32_t(~0);
		for (uint32_t i = 0; i < properties.memoryTypeCount; i++) {
			auto type = properties.memoryTypes[i];
			if ((supportedTypeBits & 1) && ((type.propertyFlags & propertyFlags) == propertyFlags)) {
				memoryTypeIndex = i;
				if(logger) logger->Log("Choosing memory type #" + std::to_string(i));
				break;
			}
			supportedTypeBits >>= 1;
		}
		if (memoryTypeIndex == ~0) {
			throw std::runtime_error("Cannot find a supported device local memory type");
		}
		return memoryTypeIndex;
	}
}
