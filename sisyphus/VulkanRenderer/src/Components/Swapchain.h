#pragma once
#include "VulkanComponent.h"
#include "Vulkan.h"

namespace Sisyphus::Rendering::Vulkan {

	class Swapchain : public IVulkanComponent<vk::SwapchainKHR> {
	public:
		void Initialize() override;
		void RegisterEventHandlers() override;
		static uuids::uuid TypeId();
		static std::string ClassName();
		static ECS::ComponentReferences Dependencies();

		vk::SwapchainKHR GetVulkanObject() const override;

		void Clear();
		void Resize();

		struct AcquireResult {
			uint32_t imageIndex;
			vk::UniqueSemaphore semaphore;
		};
		AcquireResult AcquireNextImage();

		const std::vector<vk::UniqueImageView>& GetImageViews() const;

	private:
		vk::UniqueSwapchainKHR swapchain;
		std::vector<vk::Image> swapchainImages;
		std::vector<vk::UniqueImageView> imageViews;
	};
}
