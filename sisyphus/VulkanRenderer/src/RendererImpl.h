#pragma once
#include "Renderer.h"
#include "Vulkan.h"
#include "PlatformSpecific.h"
#include "DepthBuffer.h"
#include "UniformBuffer.h"
#include "VertexBuffer.h"
#include "DebugMessenger.h"
#include "Shader.h"
#include "Utils\ILogger.h"
#include <memory>
#include <optional>
#include <unordered_map>

namespace WindowCreator {
	class Window;
}

namespace Sisyphus::Rendering::Vulkan {

	class RendererImpl : IRenderer {
	public:
		RendererImpl(const RendererCreateInfo& ci);
		~RendererImpl(); // default

		void Draw(const IDrawable& drawable) const;

		void UpdateUniformBuffer(Renderer::UniformBufferData data);
		Renderer::UniformBufferData GetUniformBufferData();

		void UpdateVertexBuffer(Renderer::VertexBufferData data);
		Renderer::VertexBufferData GetVertexBufferData();

		void CreateShader(const ShaderInfo& shaderInfo);
		bool ShaderExists(uuids::uuid id) const;
		void EnableShader(uuids::uuid id);

	private:
		void InitInstance();
		void InitDebugMessenger();
		void InitWindow();
		void InitSurface();
		void InitPhysicalDevice();
		void InitQueueFamilyIndex();
		void InitDevice();
		void InitCommandPool();
		void InitCommandBuffers();
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
		void InitVertexBuffer();
		void InitShaders();
		void InitPipeline();

		std::vector<const char*> GetInstanceLayerNames();
		Shader& GetShader(uuids::uuid id);

		RendererCreateInfo ci;
		vk::UniqueInstance instance;
		std::unique_ptr<DebugMessenger> debugMessenger;
		std::unique_ptr<WindowCreator::Window> window;
		vk::UniqueSurfaceKHR surface;
		vk::PhysicalDevice physicalDevice;
		std::optional<int> queueFamilyIndex;
		vk::UniqueDevice device;
		vk::UniqueCommandPool commandPool;
		std::vector<vk::CommandBuffer> commandBuffers;
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