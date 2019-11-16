#include "Renderer.h"
#include "RendererImpl.h"

namespace Sisyphus::Rendering::Vulkan {
	Renderer::Renderer(const RendererCreateInfo& ci) :
		impl(std::make_unique<RendererImpl>(ci))
	{
	}

	Renderer::~Renderer() = default;

	void Renderer::Draw(const IDrawable & drawable) const
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

	void Renderer::UpdateVertexBuffer(VertexBufferData data)
	{
		impl->UpdateVertexBuffer(data);
	}

	Renderer::VertexBufferData Renderer::GetVertexBufferData()
	{
		return impl->GetVertexBufferData();
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
