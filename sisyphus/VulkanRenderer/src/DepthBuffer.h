#pragma once
#include "Vulkan.h"
#include "Utils\ILogger.h"

namespace Sisyphus::Rendering::Vulkan {
	class DepthBuffer {
	public:
		struct CreateInfo {
			vk::Extent2D extent;
			vk::PhysicalDevice physicalDevice;
			vk::Device device;
			ILogger* logger;
		};

		static constexpr vk::Format format = vk::Format::eD16Unorm;

		DepthBuffer(CreateInfo ci);
		~DepthBuffer(); // default

		vk::ImageView GetImageView() const;

	private:
		void CreateImage();
		void AllocateMemory();
		void BindMemory();
		void CreateImageView();

		CreateInfo ci;
		vk::UniqueImage image;
		vk::UniqueDeviceMemory memory;
		vk::UniqueImageView imageView;
	};
}