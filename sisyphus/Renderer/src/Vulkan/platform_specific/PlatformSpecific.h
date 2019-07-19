#pragma once
#include "ActivatePlatformExtensions.h"
#include <vulkan/vulkan.hpp>
#include <vector>

namespace PlatformSpecific {

	std::vector<const char *> GetInstanceExtensionNames() {
		return{
			VK_KHR_WIN32_SURFACE_EXTENSION_NAME
		};
	}
}