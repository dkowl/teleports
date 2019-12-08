#pragma once
#include "Renderer.h"
#include "Vulkan.h"
#include "PlatformSpecific.h"
#include "DepthBuffer.h"
#include "UniformBuffer.h"
#include "VertexBuffer.h"
#include "Shader.h"
#include "ComponentManager.h"
#include "Utils\ILogger.h"
#include <memory>
#include <optional>
#include <unordered_map>

namespace Sisyphus::WindowCreator {
	class Window;
	struct WindowExtent;
}

namespace Sisyphus::Rendering::Vulkan {

	class RendererImpl : public IRenderer {
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
		void InitWindowExtent();
		void InitSurface();
		void InitQueueFamilyIndex();
		void InitDevice();
		void InitCommandPool();
		void InitFormatAndColorSpace();
		void InitSwapchain();
		void InitSwapchainImages();
		void InitImageViews();
		void InitDepthBuffer();
		void InitDescriptorSetLayout();
		void InitPipelineLayout();
		void InitDescriptorPool();
		void InitDescriptorSet();
		void InitUniformBuffer();
		void InitRenderPass();
		void InitFramebuffers();
		void InitShaders();

		// happens each Draw()
		void AdaptToSurfaceChanges();
		void InitVertexBuffer(size_t size);
		void InitCommandBuffers();
		void InitPipeline(uint32_t stride);

		Shader& GetShader(uuids::uuid id);

		RendererCreateInfo ci;
		ComponentManager componentManager;
		std::unique_ptr<WindowCreator::WindowExtent> windowExtent;
		vk::UniqueSurfaceKHR surface;
		std::optional<int> queueFamilyIndex;
		vk::UniqueDevice device;
		vk::UniqueCommandPool commandPool;
		std::vector<vk::UniqueCommandBuffer> commandBuffers;
		std::optional<vk::Format> colorFormat;
		std::optional<vk::ColorSpaceKHR> colorSpace;
		vk::UniqueSwapchainKHR swapchain;
		std::vector<vk::Image> swapchainImages;
		std::vector<vk::UniqueImageView> imageViews;
		std::unique_ptr<DepthBuffer> depthBuffer;

		vk::UniqueDescriptorSetLayout descriptorSetLayout;
		vk::UniquePipelineLayout pipelineLayout;
		vk::UniqueDescriptorPool descriptorPool;
		vk::UniqueDescriptorSet descriptorSet;
		std::unique_ptr<UniformBuffer> uniformBuffer;

		vk::UniqueRenderPass renderPass;
		std::vector<vk::UniqueFramebuffer> framebuffers;
		std::unique_ptr<VertexBuffer> vertexBuffer;
		vk::UniquePipeline pipeline;

		std::unordered_map<uuids::uuid, std::unique_ptr<Shader>> shaders;
		uuids::uuid vertexShaderId;
		uuids::uuid fragmentShaderId;

		ILogger* logger;
	};
}