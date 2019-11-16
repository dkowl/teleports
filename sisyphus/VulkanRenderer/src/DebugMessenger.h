#pragma once
#include "Vulkan.h"

namespace Sisyphus::Rendering::Vulkan {

	class DebugMessenger {
	public:
		DebugMessenger(const vk::Instance& instance);

	private:
		vk::UniqueDebugUtilsMessengerEXT debugUtilsMessenger;
	};
}