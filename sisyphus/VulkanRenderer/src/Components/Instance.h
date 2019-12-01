#pragma once
#include "Component.h"
#include "Vulkan.h"

namespace Sisyphus::Rendering::Vulkan {
	class DebugMessenger;

	class Instance : public Component {
	public:
		Instance();
		~Instance() override; // default

		void OnInitialize() override;
		ComponentType GetType() const override;
		std::vector<ComponentType> GetDependencies() const override;

		vk::Instance GetVulkanObject() const;
		operator vk::Instance() const;

	private:
		static std::vector<const char*> GetLayerNames();

		vk::UniqueInstance instance;
		std::unique_ptr<DebugMessenger> debugMessenger;
	};
}