#pragma once
#include "Vulkan.h"

class VulkanDepthBuffer {
public:
	struct CreateInfo {
		vk::Extent2D extent;
		vk::PhysicalDevice physicalDevice;
		vk::Device device;
	};

	VulkanDepthBuffer(CreateInfo ci);
	~VulkanDepthBuffer(); // default

private:
	void CreateImage();
	void AllocateMemory();

	CreateInfo ci;
	vk::UniqueImage image;
};