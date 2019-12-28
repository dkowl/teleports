#pragma once
#include "Renderer.h"
#include "Vulkan.h"
#include "PlatformSpecific.h"
#include "UniformBuffer.h"
#include "VertexBuffer.h"
#include "Shader.h"
#include "ECS\Entity.h"
#include "Utils\ILogger.h"
#include <memory>
#include <optional>
#include <unordered_map>

namespace Sisyphus::Rendering::Vulkan {

	class RendererImpl : public IRenderer, public ECS::Entity {
	public:
		RendererImpl(const RendererCreateInfo& ci);
		~RendererImpl(); // default

		void Draw(const IDrawable& drawable);

		void UpdateUniformBuffer(Renderer::UniformBufferData data);
		Renderer::UniformBufferData GetUniformBufferData();

		void CreateShader(const ShaderInfo& shaderInfo);
		bool ShaderExists(uuids::uuid id) const;
		void EnableShader(uuids::uuid id);

	private:
		void InitDescriptorSetLayout();
		void InitPipelineLayout();
		void InitDescriptorPool();
		void InitDescriptorSet();
		void InitUniformBuffer();
		void InitRenderPass();
		void InitShaders();

		// happens each Draw()
		void InitVertexBuffer(size_t size);
		void InitPipeline(uint32_t stride);

		Shader& GetShader(uuids::uuid id);

		RendererCreateInfo ci;

		vk::UniqueDescriptorSetLayout descriptorSetLayout;
		vk::UniquePipelineLayout pipelineLayout;
		vk::UniqueDescriptorPool descriptorPool;
		vk::UniqueDescriptorSet descriptorSet;
		std::unique_ptr<UniformBuffer> uniformBuffer;

		vk::UniqueRenderPass renderPass;
		std::unique_ptr<VertexBuffer> vertexBuffer;
		vk::UniquePipeline pipeline;

		std::unordered_map<uuids::uuid, std::unique_ptr<Shader>> shaders;
		uuids::uuid vertexShaderId;
		uuids::uuid fragmentShaderId;

		ILogger* logger;
	};
}