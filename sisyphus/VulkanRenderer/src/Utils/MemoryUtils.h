#pragma once
#include "Vulkan.h"

namespace Vulkan {
	uint32_t FindMemoryType(vk::PhysicalDeviceMemoryProperties properties, uint32_t supportedTypeBits, vk::MemoryPropertyFlags propertyFlags);
}