#pragma once
#include "VulkanComponent.h"
#include "Vulkan.h"

namespace Sisyphus::Rendering::Vulkan {
	class DebugMessenger;

	class Instance : public IVulkanComponent<vk::Instance> {
	public:
		Instance();
		~Instance() override; // default

		void Initialize() override;
		static uuids::uuid TypeId();
		static std::string ClassName();
		static ECS::ComponentReferences Dependencies();

		vk::Instance GetVulkanObject() const override;

	private:
		static std::vector<const char*> GetLayerNames();

		vk::UniqueInstance instance;
		std::unique_ptr<DebugMessenger> debugMessenger;
	};
}