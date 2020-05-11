#include "Pch_VulkanRenderer.h"
#include "DepthBuffer.h"
#include "MemoryUtils.h"
#include "Utils\DebugAssert.h"
#include "Utils\Throw.h"
#include "Logger/Logger.h"
#include "ECS\Entity.h"
#include "Device.h"
#include "PhysicalDevice.h"
#include "Surface.h"
#include "Events.h"
#include "Swapchain.h"

namespace Sisyphus::Rendering::Vulkan {
	SIS_DEFINE_ID(ComponentID_DepthBuffer, "110a4062db0746428d15fde397461fcb");

	void DepthBuffer::Initialize()
	{
		auto& device = Parent().GetComponent<Device>();
		auto& physicalDevice = Parent().GetComponent<PhysicalDevice>();
		auto& surface = Parent().GetComponent<Surface>();

		auto& logger = Logger();
		logger.BeginSection("DepthBuffer");
		image = CreateImage(surface.GetExtent(), device, physicalDevice);
		memory = device.AllocateAndBindImageMemory(*image);
		imageView = CreateImageView(*image, device);
		logger.EndSection();
	}

	void DepthBuffer::RegisterEventHandlers()
	{
		RegisterEventHandler<ResizeEvent, Swapchain>(std::bind(&DepthBuffer::Resize, this));
	}

	uuids::uuid DepthBuffer::TypeId()
	{
		return ComponentID_DepthBuffer;
	}

	std::string DepthBuffer::ClassName()
	{
		return "DepthBuffer";
	}

	ECS::ComponentReferences DepthBuffer::Dependencies()
	{
		return { {PhysicalDevice::TypeId()}, {Device::TypeId()}, {Surface::TypeId()} };
	}

	void DepthBuffer::Clean()
	{
		imageView.reset(nullptr);
		image.reset(nullptr);
		memory.reset(nullptr);
	}

	void DepthBuffer::Resize()
	{
		Clean();
		Initialize();
		Parent().Dispatch<ResizeEvent, DepthBuffer>();
	}

	vk::ImageView DepthBuffer::GetImageView() const
	{
		return *imageView;
	}

	vk::ImageTiling ChooseTiling(vk::Format format, vk::PhysicalDevice physicalDevice, vk::FormatFeatureFlagBits feature) {
		vk::FormatProperties formatProperties = physicalDevice.getFormatProperties(format);

		vk::ImageTiling tiling{};
		if (formatProperties.optimalTilingFeatures & feature) {
			tiling = vk::ImageTiling::eOptimal;
		}
		else if (formatProperties.linearTilingFeatures & feature) {
			tiling = vk::ImageTiling::eLinear;
		}
		else {
			SIS_THROW(vk::to_string(feature) + " is not supported for " + vk::to_string(format));
		}
		Logger().Log("Image Tiling for " + vk::to_string(feature) + " in " + vk::to_string(format) + " is " + vk::to_string(tiling));
		return tiling;
	}

	vk::UniqueImage DepthBuffer::CreateImage(vk::Extent2D extent, vk::Device device, vk::PhysicalDevice physicalDevice)
	{
		vk::ImageTiling tiling = ChooseTiling(format, physicalDevice, vk::FormatFeatureFlagBits::eDepthStencilAttachment);

		vk::ImageCreateInfo imageCreateInfo(
			{},
			vk::ImageType::e2D,
			format,
			vk::Extent3D(extent, 1),
			1,
			1,
			vk::SampleCountFlagBits::e1,
			tiling,
			vk::ImageUsageFlagBits::eDepthStencilAttachment,
			vk::SharingMode::eExclusive
		);

		return device.createImageUnique(imageCreateInfo);
	}

	vk::UniqueImageView DepthBuffer::CreateImageView(vk::Image image, vk::Device device)
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
			image,
			vk::ImageViewType::e2D,
			format,
			componentMapping,
			subresourceRange
		};

		return device.createImageViewUnique(imageViewCreateInfo);
	}
}
