#pragma once
#include "VulkanComponent.h"
#include "Vulkan.h"

namespace Sisyphus::Rendering::Vulkan {

	class Device : public IVulkanComponent<vk::Device> {
	public:
		void Initialize() override;
		static uuids::uuid TypeId();
		static std::string ClassName();
		static ECS::ComponentReferences Dependencies();

		vk::Device GetVulkanObject() const override;

		vk::Queue GetGraphicsQueue() const;
		vk::Queue GetPresentQueue() const;

		void InitCommandBuffers();
		void ResetCommandPool();
		vk::CommandBuffer GetCommandBuffer() const;

		vk::UniqueDeviceMemory AllocateImageMemory(vk::Image image);

	private:
		vk::UniqueDevice device;
		vk::Queue graphicsQueue;
		vk::Queue presentQueue;
		vk::UniqueCommandPool commandPool;
		std::vector<vk::UniqueCommandBuffer> commandBuffers;
	};
}