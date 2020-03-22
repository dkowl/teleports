#include "Pch_VulkanRenderer.h"
#include "Utils/Logger.h"
#include "Swapchain.h"
#include "Surface.h"
#include "Device.h"
#include "PhysicalDevice.h"
#include "ECS/Entity.h"
#include "Events.h"

namespace Sisyphus::Rendering::Vulkan {
	SIS_DEFINE_ID(ComponentID_Swapchain, "50add9e4abaf44b49728f8d1958881bc");

	constexpr uint64_t timeout = 100000000; // 100ms

	void Swapchain::Initialize()
	{
		auto& surface = Parent().GetComponent<Surface>();
		auto& logger = Logger();

		constexpr int desiredMinImageCount = 3; // triple buffering
		auto physicalDevice = Parent().GetComponent<PhysicalDevice>().GetVulkanObject();
		auto surfaceCapabilites = physicalDevice.getSurfaceCapabilitiesKHR(surface);
		logger.Log("Surface minImageCount: " + std::to_string(surfaceCapabilites.minImageCount));
		logger.Log("Surface maxImageCount: " + std::to_string(surfaceCapabilites.maxImageCount));
		if (
			surfaceCapabilites.minImageCount > desiredMinImageCount ||
			surfaceCapabilites.maxImageCount < desiredMinImageCount)
		{
			SIS_THROW("Surface does not support three image buffers");
		}

		logger.Log("Surface extent: (w: " + std::to_string(surfaceCapabilites.currentExtent.width) +
			", h: " + std::to_string(surfaceCapabilites.currentExtent.height) + ")");

		if (!(surfaceCapabilites.supportedTransforms & vk::SurfaceTransformFlagBitsKHR::eIdentity)) {
			SIS_THROW("Identity surface transform not supported");
		}

		logger.Log("Supported composite alpha: " + vk::to_string(surfaceCapabilites.supportedCompositeAlpha));
		if (!(surfaceCapabilites.supportedCompositeAlpha & vk::CompositeAlphaFlagBitsKHR::eOpaque)) {
			SIS_THROW("Surface opaque composite alpha mode not supported");
		}

		auto desiredPresentMode = vk::PresentModeKHR::eFifoRelaxed;
		auto supportedPresentModes = physicalDevice.getSurfacePresentModesKHR(surface);
		bool modeFound = false;
		logger.Log("Supported present modes: ");
		for (auto&& mode : supportedPresentModes) {
			logger.Log(vk::to_string(mode));
			if (mode == desiredPresentMode) {
				modeFound = true;
			}
		}
		if (!modeFound) {
			SIS_THROW("Present mode " + vk::to_string(desiredPresentMode) + " not supported by GPU");
		}

		auto format = surface.GetFormat();
		auto colorSpace = surface.GetColorSpace();

		vk::SwapchainCreateInfoKHR swapchainCreateInfo(
			{},
			surface,
			desiredMinImageCount,
			format,
			colorSpace,
			surfaceCapabilites.currentExtent,
			1,
			vk::ImageUsageFlagBits::eColorAttachment,
			vk::SharingMode::eExclusive,
			0,
			nullptr,
			vk::SurfaceTransformFlagBitsKHR::eIdentity,
			vk::CompositeAlphaFlagBitsKHR::eOpaque,
			desiredPresentMode,
			false,
			nullptr
		);

		auto device = Parent().GetComponent<Device>().GetVulkanObject();
		swapchain = device.createSwapchainKHRUnique(swapchainCreateInfo); 
		swapchainImages = device.getSwapchainImagesKHR(*swapchain);

		vk::ComponentMapping componentMapping{
			vk::ComponentSwizzle::eR,
			vk::ComponentSwizzle::eG,
			vk::ComponentSwizzle::eB,
			vk::ComponentSwizzle::eA,
		};
		vk::ImageSubresourceRange subresourceRange{
			vk::ImageAspectFlagBits::eColor,
			0,
			1,
			0,
			1
		};
		imageViews.reserve(swapchainImages.size());
		for (auto&& image : swapchainImages) {
			vk::ImageViewCreateInfo imageViewCreateInfo(
				{},
				image,
				vk::ImageViewType::e2D,
				surface.GetFormat(),
				componentMapping,
				subresourceRange
			);
			imageViews.emplace_back(device.createImageViewUnique(imageViewCreateInfo));
		}
	}
	void Swapchain::RegisterEventHandlers()
	{
		RegisterEventHandler<ResizeEvent, Surface>(std::bind(&Swapchain::Resize, this));
	}
	uuids::uuid Swapchain::TypeId()
	{
		return ComponentID_Swapchain;
	}
	std::string Swapchain::ClassName()
	{
		return "Swapchain";
	}
	ECS::ComponentReferences Swapchain::Dependencies()
	{
		return { {PhysicalDevice::TypeId()}, {Surface::TypeId()}, {Device::TypeId()} };
	}
	vk::SwapchainKHR Swapchain::GetVulkanObject() const
	{
		return *swapchain;
	}
	void Swapchain::Clear()
	{
		imageViews.clear();
		swapchainImages.clear();
		swapchain.reset(nullptr);
	}
	void Swapchain::Resize()
	{
		Clear();
		Initialize();
		Parent().Dispatch<ResizeEvent, Swapchain>();
	}
	Swapchain::AcquireResult Swapchain::AcquireNextImage()
	{
		auto device = Parent().GetComponent<Device>().GetVulkanObject();
		AcquireResult result;
		result.semaphore = device.createSemaphoreUnique({});

		auto callResult = device.acquireNextImageKHR(*swapchain, timeout, *result.semaphore, nullptr);
		SIS_THROWASSERT_MSG(callResult.result == vk::Result::eSuccess, "Failed to acquire an image buffer!");
		result.imageIndex = callResult.value;

		return std::move(result);
	}
	const std::vector<vk::UniqueImageView>& Swapchain::GetImageViews() const
	{
		return imageViews;
	}
}