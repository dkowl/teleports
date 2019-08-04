#include "DepthBuffer.h"
#include "MemoryUtils.h"
#include "Utils\BreakAssert.h"
#include <iostream>

namespace Vulkan {
	DepthBuffer::DepthBuffer(CreateInfo ci) :
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

	DepthBuffer::~DepthBuffer() = default;

	void DepthBuffer::CreateImage()
	{
		vk::FormatProperties formatProperties = ci.physicalDevice.getFormatProperties(format);

		vk::ImageTiling tiling;
		if (formatProperties.optimalTilingFeatures & vk::FormatFeatureFlagBits::eDepthStencilAttachment) {
			tiling = vk::ImageTiling::eOptimal;
		}
		else if (formatProperties.linearTilingFeatures & vk::FormatFeatureFlagBits::eDepthStencilAttachment) {
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

	void DepthBuffer::AllocateMemory()
	{
		vk::PhysicalDeviceMemoryProperties memoryProperties = ci.physicalDevice.getMemoryProperties();

		vk::MemoryRequirements memoryRequirements = ci.device.getImageMemoryRequirements(*image);
		uint32_t supportedTypeBits = memoryRequirements.memoryTypeBits;

		auto memoryTypeIndex = FindMemoryType(memoryProperties, supportedTypeBits, vk::MemoryPropertyFlagBits::eDeviceLocal);

		std::cout << "\t" << memoryRequirements.size << " bytes of GPU memory required\n";
		std::cout << "\tAlignment: " << memoryRequirements.alignment << std::endl;
		memory = ci.device.allocateMemoryUnique(vk::MemoryAllocateInfo(memoryRequirements.size, memoryTypeIndex));
	}

	void DepthBuffer::BindMemory()
	{
		BreakAssert(image);
		BreakAssert(memory);

		ci.device.bindImageMemory(*image, *memory, 0);
	}

	void DepthBuffer::CreateImageView()
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
}
