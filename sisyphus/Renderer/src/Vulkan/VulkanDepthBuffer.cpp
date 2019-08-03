#include "VulkanDepthBuffer.h"
#include "Utils\BreakAssert.h"
#include <iostream>

void InspectMemoryProperties(vk::PhysicalDeviceMemoryProperties properties) {
	std::cout << "\tMemory types:\n";
	for (uint32_t i = 0; i < properties.memoryTypeCount; i++) {
		auto type = properties.memoryTypes[i];
		std::cout << "\t\t#" << i << std::endl;
		std::cout << "\t\t\tHeap index: " << type.heapIndex << std::endl;
		std::cout << "\t\t\tProperties: " << vk::to_string(type.propertyFlags) << std::endl;
	}

	std::cout << "\tHeaps:\n";
	for (uint32_t i = 0; i < properties.memoryHeapCount; i++) {
		auto heap = properties.memoryHeaps[i];
		std::cout << "\t\t#" << i << std::endl;
		std::cout << "\t\t\tSize: " << heap.size << std::endl;
		std::cout << "\t\t\tFlags: " << vk::to_string(heap.flags) << std::endl;
	}
}

VulkanDepthBuffer::VulkanDepthBuffer(CreateInfo ci):
	ci(ci)
{
	std::cout << "Depth Buffer:\n";
	CreateImage();
	std::cout << "\tImage created!\n";
	AllocateMemory();
	std::cout << "\tMemory allocated!\n";
	BindMemory();
	std::cout << "\tMemory bound!\n";
	CreateImageView();
	std::cout << "\tImage View created!\n";
}

VulkanDepthBuffer::~VulkanDepthBuffer() = default;

void VulkanDepthBuffer::CreateImage()
{
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
		format,
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
	vk::PhysicalDeviceMemoryProperties memoryProperties = ci.physicalDevice.getMemoryProperties();
	InspectMemoryProperties(memoryProperties);

	vk::MemoryRequirements memoryRequirements = ci.device.getImageMemoryRequirements(*image);
	uint32_t supportedTypeBits = memoryRequirements.memoryTypeBits;
	uint32_t memoryTypeIndex = uint32_t(~0);
	for (uint32_t i = 0; i < memoryProperties.memoryTypeCount; i++) {
		auto type = memoryProperties.memoryTypes[i];
		if ((supportedTypeBits & 1) && (type.propertyFlags & vk::MemoryPropertyFlagBits::eDeviceLocal)) {
			memoryTypeIndex = i;
			std::cout << "\tChoosing memory type #" << i << std::endl;
			break;
		}
		supportedTypeBits >>= 1;
	}
	if (memoryTypeIndex == ~0) {
		throw std::runtime_error("Cannot find a supported device local memory type");
	}
	std::cout << "\t" << memoryRequirements.size << " bytes of GPU memory required\n";
	std::cout << "\tAlignment: " << memoryRequirements.alignment << std::endl;
	memory = ci.device.allocateMemoryUnique(vk::MemoryAllocateInfo(memoryRequirements.size, memoryTypeIndex));
}

void VulkanDepthBuffer::BindMemory()
{
	BreakAssert(image);
	BreakAssert(memory);

	ci.device.bindImageMemory(*image, *memory, 0);
}

void VulkanDepthBuffer::CreateImageView()
{
	vk::ComponentMapping componentMapping(
		vk::ComponentSwizzle::eR, 
		vk::ComponentSwizzle::eG, 
		vk::ComponentSwizzle::eB, 
		vk::ComponentSwizzle::eA
	);
	vk::ImageSubresourceRange subresourceRange(vk::ImageAspectFlagBits::eDepth, 0, 1, 0, 1);

	vk::ImageViewCreateInfo imageViewCreateInfo{
		{},
		*image,
		vk::ImageViewType::e2D,
		format,
		componentMapping,
		subresourceRange
	};

	imageView = ci.device.createImageViewUnique(imageViewCreateInfo);
}
