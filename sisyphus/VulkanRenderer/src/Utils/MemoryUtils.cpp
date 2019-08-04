#include "MemoryUtils.h"
#include <iostream>

namespace Vulkan {
	void InspectMemoryProperties(vk::PhysicalDeviceMemoryProperties properties) {
		std::cout << "\tMemory types:\n";
		for (uint32_t i = 0; i < properties.memoryTypeCount; i++) {
			auto type = properties.memoryTypes[i];
			std::cout << "\t\t#" << i << std::endl;
			std::cout << "\t\t\tHeap index: " << type.heapIndex << std::endl;
			std::cout << "\t\t\tProperties: " << vk::to_string(type.propertyFlags) << std::endl;
		}

		std::cout << "\tHeaps:\n";
		for (uint32_t i = 0; i < properties.memoryHeapCount; i++) {
			auto heap = properties.memoryHeaps[i];
			std::cout << "\t\t#" << i << std::endl;
			std::cout << "\t\t\tSize: " << heap.size << std::endl;
			std::cout << "\t\t\tFlags: " << vk::to_string(heap.flags) << std::endl;
		}
	}

	uint32_t FindMemoryType(vk::PhysicalDeviceMemoryProperties properties, uint32_t supportedTypeBits, vk::MemoryPropertyFlags propertyFlags)
	{
		InspectMemoryProperties(properties);

		uint32_t memoryTypeIndex = uint32_t(~0);
		for (uint32_t i = 0; i < properties.memoryTypeCount; i++) {
			auto type = properties.memoryTypes[i];
			if ((supportedTypeBits & 1) && ((type.propertyFlags & propertyFlags) == propertyFlags)) {
				memoryTypeIndex = i;
				std::cout << "\tChoosing memory type #" << i << std::endl;
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
