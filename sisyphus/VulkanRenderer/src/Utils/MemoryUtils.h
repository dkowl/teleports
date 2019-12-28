#pragma once
#include "Vulkan.h"

namespace Sisyphus::Rendering::Vulkan {
	uint32_t FindMemoryType(
		vk::PhysicalDeviceMemoryProperties properties, 
		uint32_t supportedTypeBits,
		vk::MemoryPropertyFlags propertyFlags);
}