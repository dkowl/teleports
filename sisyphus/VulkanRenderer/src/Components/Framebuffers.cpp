#include "Pch_VulkanRenderer.h"
#include "Framebuffers.h"
#include "ECS\Entity.h"
#include "Surface.h"
#include "DepthBuffer.h"
#include "Swapchain.h"
#include "Device.h"
#include "Events.h"

namespace Sisyphus::Rendering::Vulkan {
	SIS_DEFINE_ID(ComponentID_Framebuffers, "7f95f4a63d16400595144db614c492ce");

	Framebuffers::Framebuffers(vk::RenderPass inRenderPass):
		renderPass(inRenderPass)
	{
	}
	void Framebuffers::Initialize()
	{
		framebuffers.clear();

		auto surfaceExtent = Parent().GetComponent<Surface>().GetExtent();
		vk::ImageView attachments[2];
		attachments[1] = Parent().GetComponent<DepthBuffer>().GetImageView();

		for (const auto& imageView : Parent().GetComponent<Swapchain>().GetImageViews()) {
			attachments[0] = *imageView;
			vk::FramebufferCreateInfo framebufferCreateInfo{
				{},
				renderPass,
				2,
				attachments,
				surfaceExtent.width,
				surfaceExtent.height,
				1
			};

			framebuffers.push_back(Parent().GetComponent<Device>().GetVulkanObject().createFramebufferUnique(framebufferCreateInfo));
		}

		Logger::Get().Log(std::to_string(framebuffers.size()) + " Framebuffers initialized!");
	}
	void Framebuffers::RegisterEventHandlers()
	{
		RegisterEventHandler<ResizeEvent, DepthBuffer>(std::bind(&Framebuffers::Resize, this));
	}
	uuids::uuid Framebuffers::TypeId()
	{
		return ComponentID_Framebuffers;
	}
	std::string Framebuffers::ClassName()
	{
		return "Framebuffers";
	}
	ECS::ComponentReferences Framebuffers::Dependencies()
	{
		return { {Surface::TypeId()}, {DepthBuffer::TypeId()}, {Swapchain::TypeId()}, {Device::TypeId()} };
	}
	void Framebuffers::Clean()
	{
		framebuffers.clear();
	}
	void Framebuffers::Resize()
	{
		Clean();
		Initialize();
	}
	const std::vector<vk::UniqueFramebuffer>& Framebuffers::GetFramebuffers() const
	{
		return framebuffers;
	}
}