#include "VulkanDepthBuffer.h"
#include <iostream>

void InspectMemoryProperties(vk::PhysicalDeviceMemoryProperties properties) {
	std::cout << "\tMemory types:\n";
	for (int i = 0; i < properties.memoryTypeCount; i++) {
		auto type = properties.memoryTypes[i];
		std::cout << "\t\t#" << i << std::endl;
		std::cout << "\t\t\tHeap index: " << type.heapIndex << std::endl;
		std::cout << "\t\t\tProperties: " << vk::to_string(type.propertyFlags) << std::endl;
	}
}

VulkanDepthBuffer::VulkanDepthBuffer(CreateInfo ci):
	ci(ci)
{
	std::cout << "Depth Buffer:\n";
	CreateImage();
	std::cout << "\tImage created!\n";
	AllocateMemory();
	std::cout << "\tMemory allocated\n";
}

VulkanDepthBuffer::~VulkanDepthBuffer() = default;

void VulkanDepthBuffer::CreateImage()
{
	constexpr vk::Format format = vk::Format::eD16Unorm;
	vk::FormatProperties formatProperties = ci.physicalDevice.getFormatProperties(format);

	vk::ImageTiling tiling;
	if (formatProperties.optimalTilingFeatures & vk::FormatFeatureFlagBits::eDepthStencilAttachment) {
		tiling = vk::ImageTiling::eOptimal;
	}
	else if (formatProperties.linearTilingFeatures & vk::FormatFeatureFlagBits::eDepthStencilAttachment){
		tiling = vk::ImageTiling::eLinear;
	}
	else {
		throw std::runtime_error("DepthStencilAttachment is not supported for " + vk::to_string(format));
	}
	std::cout << "\tImage Tiling: " << vk::to_string(tiling) << std::endl;

	vk::ImageCreateInfo imageCreateInfo(
		{},
		vk::ImageType::e2D,
		vk::Format::eD16Unorm,
		vk::Extent3D(ci.extent, 1),
		1,
		1,
		vk::SampleCountFlagBits::e1,
		tiling,
		vk::ImageUsageFlagBits::eDepthStencilAttachment,
		vk::SharingMode::eExclusive
	);

	image = ci.device.createImageUnique(imageCreateInfo);
}

void VulkanDepthBuffer::AllocateMemory()
{
	vk::MemoryRequirements memoryRequirements = ci.device.getImageMemoryRequirements(*image);
	vk::PhysicalDeviceMemoryProperties memoryProperties = ci.physicalDevice.getMemoryProperties();
	InspectMemoryProperties(memoryProperties);

}
