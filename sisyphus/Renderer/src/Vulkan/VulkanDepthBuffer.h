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
	void BindMemory();
	void CreateImageView();

	static constexpr vk::Format format = vk::Format::eD16Unorm;
	CreateInfo ci;
	vk::UniqueImage image;
	vk::UniqueDeviceMemory memory;
	vk::UniqueImageView imageView;
};