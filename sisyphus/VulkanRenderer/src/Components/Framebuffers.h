#pragma once
#include "Vulkan.h"
#include "ECS\Component.h"

namespace Sisyphus::Rendering::Vulkan {

	class Framebuffers : public ECS::IComponent {
	public:
		Framebuffers(vk::RenderPass inRenderPass);

		void Initialize() override;
		void RegisterEventHandlers() override;
		static uuids::uuid TypeId();
		static std::string ClassName();
		static ECS::ComponentReferences Dependencies();

		void Clean();
		void Resize();

		const std::vector<vk::UniqueFramebuffer>& GetFramebuffers() const;

	private:
		vk::RenderPass renderPass;
		std::vector<vk::UniqueFramebuffer> framebuffers;
	};
}