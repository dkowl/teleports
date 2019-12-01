#include "DepthBuffer.h"
#include "MemoryUtils.h"
#include "Utils\BreakAssert.h"
#include "Utils\Throw.h"
#include <iostream>

namespace Sisyphus::Rendering::Vulkan {
	DepthBuffer::DepthBuffer(CreateInfo ci) :
		ci(ci)
	{
		if (ci.logger == nullptr) {
			SIS_THROW("Logger cannot be null");
		}

		CreateImage();
		ci.logger->Log("Image created!");
		AllocateMemory();
		ci.logger->Log("Memory allocated!");
		BindMemory();
		ci.logger->Log("Memory bound!");
		CreateImageView();
		ci.logger->Log("Image View created!");
	}

	DepthBuffer::~DepthBuffer() = default;

	vk::ImageView DepthBuffer::GetImageView() const
	{
		return *imageView;
	}

	void DepthBuffer::CreateImage()
	{
		vk::FormatProperties formatProperties = ci.physicalDevice.getFormatProperties(format);

		vk::ImageTiling tiling{};
		if (formatProperties.optimalTilingFeatures & vk::FormatFeatureFlagBits::eDepthStencilAttachment) {
			tiling = vk::ImageTiling::eOptimal;
		}
		else if (formatProperties.linearTilingFeatures & vk::FormatFeatureFlagBits::eDepthStencilAttachment) {
			tiling = vk::ImageTiling::eLinear;
		}
		else {
			SIS_THROW("DepthStencilAttachment is not supported for " + vk::to_string(format));
		}
		ci.logger->Log("Image Tiling: " + vk::to_string(tiling));

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

		auto memoryTypeIndex = FindMemoryType(memoryProperties, supportedTypeBits, vk::MemoryPropertyFlagBits::eDeviceLocal, ci.logger);

		ci.logger->Log(std::to_string(memoryRequirements.size) + " bytes of GPU memory required");
		ci.logger->Log("Alignment: " + std::to_string(memoryRequirements.alignment));
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
