#include "Renderer.h"
#include "RendererImpl.h"

Vulkan::Renderer::Renderer(CreateInfo ci):
	impl(std::make_unique<RendererImpl>(ci))
{
}

Vulkan::Renderer::~Renderer() = default;
