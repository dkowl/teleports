#pragma once
#include "Vulkan.h"
#include "Utils/Throw.h"
#include <vector>

namespace Sisyphus::Rendering::Vulkan {
	namespace PlatformSpecific {

		inline std::vector<const char*> GetInstanceExtensionNames() {
#ifdef SIS_WINDOWS
			return{
				VK_KHR_WIN32_SURFACE_EXTENSION_NAME
			};
#else
			SIS_THROW("Unsupported platform");
			return {};
#endif
		}
	}
}