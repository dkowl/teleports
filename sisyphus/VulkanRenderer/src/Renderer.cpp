#include "Renderer.h"
#include "RendererImpl.h"

namespace Vulkan {
	Renderer::Renderer(CreateInfo ci) :
		impl(std::make_unique<RendererImpl>(ci))
	{
	}

	Renderer::~Renderer() = default;

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

	uuids::uuid Renderer::CreateShader(const std::string& code, ShaderType type)
	{
		return impl->CreateShader(code, type);
	}
	void Renderer::EnableShader(uuids::uuid id)
	{
		impl->EnableShader(id);
	}
}
