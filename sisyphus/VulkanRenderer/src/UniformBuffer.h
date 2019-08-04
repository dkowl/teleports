#pragma once
#include "Vulkan.h"

namespace Vulkan {

	class UniformBuffer {
	public:
		struct CreateInfo {
			int size;
			vk::Device device;
		};

		UniformBuffer(CreateInfo ci);
		~UniformBuffer(); // default

	private:
		void CreateBuffer();

		CreateInfo ci;
		vk::UniqueBuffer buffer;
	};
}
