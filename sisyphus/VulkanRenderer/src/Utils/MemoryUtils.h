#pragma once
#include "Vulkan.h"
#include "Utils\ILogger.h"

namespace Vulkan {
	uint32_t FindMemoryType(
		vk::PhysicalDeviceMemoryProperties properties, 
		uint32_t supportedTypeBits,
		vk::MemoryPropertyFlags propertyFlags, 
		ILogger* logger);
}