#include "Renderer.h"
#include "RendererImpl.h"

namespace Sisyphus::Rendering::Vulkan {
	Renderer::Renderer(const RendererCreateInfo& ci) :
		impl(std::make_unique<RendererImpl>(ci))
	{
	}

	Renderer::~Renderer() = default;

	void Renderer::Draw(const IDrawable & drawable)
	{
		impl->Draw(drawable);
	}

	void Renderer::UpdateUniformBuffer(UniformBufferData data)
	{
		impl->UpdateUniformBuffer(data);
	}

	Renderer::UniformBufferData Renderer::GetUniformBufferData()
	{
		return impl->GetUniformBufferData();
	}

	void Renderer::CreateShader(const ShaderInfo& shaderInfo)
	{
		return impl->CreateShader(shaderInfo);
	}
	void Renderer::EnableShader(uuids::uuid id)
	{
		impl->EnableShader(id);
	}
}
