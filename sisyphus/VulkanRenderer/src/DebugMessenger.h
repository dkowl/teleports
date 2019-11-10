#pragma once
#include "Vulkan.h"

namespace Vulkan {

	class DebugMessenger {
	public:
		DebugMessenger(const vk::Instance& instance);

	private:
		vk::UniqueDebugUtilsMessengerEXT debugUtilsMessenger;
	};
}