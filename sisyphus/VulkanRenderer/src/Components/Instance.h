#pragma once
#include "Component.h"
#include "Vulkan.h"

namespace Sisyphus::Rendering::Vulkan {
	class DebugMessenger;

	class Instance : public IVulkanComponent<vk::Instance> {
	public:
		Instance();
		~Instance() override; // default

		void Initialize(const ComponentManager & componentManager) override;
		static uuids::uuid TypeId();
		static std::string ClassName();
		static Dependencies Dependencies();

		vk::Instance GetVulkanObject() const override;
		operator vk::Instance() const override;

	private:
		static std::vector<const char*> GetLayerNames();

		vk::UniqueInstance instance;
		std::unique_ptr<DebugMessenger> debugMessenger;
	};
}