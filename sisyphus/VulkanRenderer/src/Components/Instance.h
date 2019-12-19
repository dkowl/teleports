#pragma once
#include "ECS/Component.h"
#include "Vulkan.h"

namespace Sisyphus::Rendering::Vulkan {
	class DebugMessenger;

	class Instance : public ECS::IVulkanComponent<vk::Instance> {
	public:
		Instance();
		~Instance() override; // default

		void Initialize(const ECS::Entity& entity) override;
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