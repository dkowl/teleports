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

	void Renderer::CreateShader(uuids::uuid id, const std::string& code, ShaderType type)
	{
		impl->CreateShader(id, code, type);
	}
}
