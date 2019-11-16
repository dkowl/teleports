#include "RendererFactory.h"
#include "VulkanRenderer/Renderer.h"

namespace Rendering {
	std::unique_ptr<IRenderer> RendererFactory::Create(const RendererCreateInfo& ci)
	{
		if (ci.type != RendererType::Vulkan) {
			throw std::runtime_error("Only the Vulkan renderer is supported at the moment");
		}

		return std::make_unique<Vulkan::Renderer>(ci);
	}
}
