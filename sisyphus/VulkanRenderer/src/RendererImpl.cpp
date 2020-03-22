#include "Pch_VulkanRenderer.h"
#include "RendererImpl.h"
#include "Utils\DebugAssert.h"
#include "Utils\UuidGenerator.h"
#include "Utils\Logger.h"
#include "Utils\Throw.h"
#include "VulkanUtils.h"
#include "Instance.h"
#include "PhysicalDevice.h"
#include "Surface.h"
#include "Device.h"
#include "Swapchain.h"
#include "DepthBuffer.h"
#include "Framebuffers.h"

namespace wc = Sisyphus::WindowCreator;

namespace Sisyphus::Rendering::Vulkan {

	constexpr uint64_t timeout = 1000000000; // 1000ms

	RendererImpl::RendererImpl(const RendererCreateInfo& ci) :
		ci(ci),
		descriptorSetLayout(nullptr),
		pipelineLayout(nullptr),
		descriptorPool(nullptr),
		descriptorSet(nullptr),
		uniformBuffer(nullptr),
		renderPass(nullptr),
		vertexBuffer(nullptr),
		pipeline(nullptr),
		logger(&Logger())
	{
		if (logger == nullptr) {
			SIS_THROW("Logger not found");
		}

		EnumerateInstanceLayerProperties();

		InitComponent<Instance>();
		InitComponent<PhysicalDevice>();
		InitComponent<Surface>(ci.window);
		InitComponent<Device>();
		InitComponent<Swapchain>();
		InitComponent<DepthBuffer>();

		InitDescriptorSetLayout();
		logger->Log("Descriptor Set Layout initialized!");
		InitPipelineLayout();
		logger->Log("Pipeline Layout initialized!");
		InitDescriptorPool();
		logger->Log("Descriptor Pool initialized!");
		InitDescriptorSet();
		logger->Log("Descriptor Set initialized!");

		logger->BeginSection("Uniform Buffer");
		InitUniformBuffer();
		logger->Log("Uniform Buffer initialized!");
		logger->EndSection();

		InitRenderPass();
		logger->Log("Render Pass initialized!");		

		InitComponent<Framebuffers>(*renderPass);

		InitShaders();
	}

	RendererImpl::~RendererImpl() = default;

	void RendererImpl::Draw(const IDrawable & drawable)
	{
		SIS_DEBUGASSERT(renderPass);
		SIS_DEBUGASSERT(descriptorSet);
		auto& surface = GetComponent<Surface>();
		surface.DetectResize();
		vk::Extent2D surfaceExtent = surface.GetExtent();

		auto& deviceComponent = GetComponent<Device>();
		auto device = deviceComponent.GetVulkanObject();
		auto& swapchainComponent = GetComponent<Swapchain>();
		auto swapchain = swapchainComponent.GetVulkanObject();
		const auto& framebuffers = GetComponent<Framebuffers>().GetFramebuffers();

		InitPipeline(drawable.GetVertexStride());
		SIS_DEBUGASSERT(pipeline);

		InitVertexBuffer(drawable.GetVertexBufferSize());
		SIS_DEBUGASSERT(vertexBuffer);
		vertexBuffer->GetDeviceData().Set(drawable.GetVertexData());

		auto acquireResult = swapchainComponent.AcquireNextImage();
		SIS_THROWASSERT_MSG(
			acquireResult.imageIndex < framebuffers.size(),
			"Acquired image index (" + std::to_string(acquireResult.imageIndex) +
			") higher that the number of available framebuffers (" + std::to_string(framebuffers.size()) + ")");

		deviceComponent.InitCommandBuffers();
		auto commandBuffer = deviceComponent.GetCommandBuffer();
		commandBuffer.begin(vk::CommandBufferBeginInfo{});

		vk::ClearValue clearValues[2];
		clearValues[0].color = vk::ClearColorValue(std::array<float, 4>{0.9f, 0.8f, 0.7f, 0.9f});
		clearValues[1].depthStencil = vk::ClearDepthStencilValue(1.0f, 0);
		vk::RenderPassBeginInfo renderPassBeginInfo{
			*renderPass,
			*framebuffers[acquireResult.imageIndex],
			vk::Rect2D(vk::Offset2D(0, 0), surfaceExtent),
			2,
			clearValues
		};
		commandBuffer.beginRenderPass(renderPassBeginInfo, vk::SubpassContents::eInline);
		commandBuffer.bindPipeline(vk::PipelineBindPoint::eGraphics, *pipeline);
		commandBuffer.bindDescriptorSets(vk::PipelineBindPoint::eGraphics, *pipelineLayout, 0, *descriptorSet, nullptr);
		commandBuffer.bindVertexBuffers(0, vertexBuffer->GetBuffer(), { 0 });
		commandBuffer.setViewport(0, vk::Viewport(
			0.0f, 0.0f, 
			static_cast<float>(surfaceExtent.width), static_cast<float>(surfaceExtent.height),
			0.0f, 1.0f));
		commandBuffer.setScissor(0, vk::Rect2D(vk::Offset2D(0, 0), surfaceExtent));

		commandBuffer.draw(drawable.GetVertexCount(), 1, 0, 0);
		commandBuffer.endRenderPass();
		commandBuffer.end();

		vk::UniqueFence drawFence = device.createFenceUnique({});

		vk::PipelineStageFlags waitDestinationStageMask(vk::PipelineStageFlagBits::eColorAttachmentOutput);
		vk::SubmitInfo submitInfo(1, &*acquireResult.semaphore, &waitDestinationStageMask, 1, &commandBuffer);

		vk::Queue graphicsQueue = deviceComponent.GetGraphicsQueue();
		vk::Queue presentQueue = deviceComponent.GetPresentQueue();

		graphicsQueue.submit(submitInfo, *drawFence);

		auto waitResult = device.waitForFences(*drawFence, VK_TRUE, timeout);
		if (waitResult == vk::Result::eTimeout) {
			logger->Log("Draw timeout!");
		}
		else if (waitResult == vk::Result::eSuccess) {
			logger->Log("Draw successful!");
		}
		else {
			SIS_DEBUGASSERT_MSG(false, "Unexpected Draw result!");
		}
		vk::PresentInfoKHR presentInfo{
			0,
			nullptr,
			1,
			&swapchain,
			&acquireResult.imageIndex
		};
		presentQueue.presentKHR(presentInfo);

		deviceComponent.ResetCommandPool();
	}

	void RendererImpl::InitDescriptorSetLayout()
	{
		vk::DescriptorSetLayoutBinding binding{
			0,
			vk::DescriptorType::eUniformBuffer,
			1,
			vk::ShaderStageFlagBits::eVertex
		};

		descriptorSetLayout = GetComponent<Device>().GetVulkanObject().createDescriptorSetLayoutUnique(vk::DescriptorSetLayoutCreateInfo(
			{},
			1,
			&binding
		));
	}

	void RendererImpl::InitPipelineLayout()
	{
		SIS_DEBUGASSERT(descriptorSetLayout);

		pipelineLayout = GetComponent<Device>().GetVulkanObject().createPipelineLayoutUnique(vk::PipelineLayoutCreateInfo(
			{},
			1,
			&(*descriptorSetLayout)
		));		
	}

	void RendererImpl::InitDescriptorPool()
	{
		vk::DescriptorPoolSize poolSize(vk::DescriptorType::eUniformBuffer, 1);

		vk::DescriptorPoolCreateInfo descriptorPoolCreateInfo(
			{vk::DescriptorPoolCreateFlagBits::eFreeDescriptorSet},
			1,
			1,
			&poolSize
		);
		
		descriptorPool = GetComponent<Device>().GetVulkanObject().createDescriptorPoolUnique(descriptorPoolCreateInfo);
	}

	void RendererImpl::InitDescriptorSet()
	{
		SIS_DEBUGASSERT(descriptorPool);

		vk::DescriptorSetAllocateInfo allocateInfo(
			*descriptorPool,
			1,
			&*descriptorSetLayout
		);

		descriptorSet = std::move(GetComponent<Device>().GetVulkanObject().allocateDescriptorSetsUnique(allocateInfo).front());
	}

	void RendererImpl::InitUniformBuffer()
	{
		SIS_DEBUGASSERT(descriptorSet);

		auto physicalDevice = GetComponent<PhysicalDevice>().GetVulkanObject();

		UniformBuffer::CreateInfo createInfo{
			sizeof(Renderer::UniformBufferData),
			GetComponent<Device>(),
			physicalDevice,
			*descriptorSet,
			logger
		};

		uniformBuffer = std::make_unique<UniformBuffer>(createInfo);
	}

	void RendererImpl::InitRenderPass()
	{
		vk::AttachmentDescription attachmentDescriptions[2];

		// color
		attachmentDescriptions[0] = vk::AttachmentDescription(
			{},
			GetComponent<Surface>().GetFormat(),
			vk::SampleCountFlagBits::e1,
			vk::AttachmentLoadOp::eClear,
			vk::AttachmentStoreOp::eStore,
			vk::AttachmentLoadOp::eDontCare,
			vk::AttachmentStoreOp::eDontCare,
			vk::ImageLayout::eUndefined,
			vk::ImageLayout::ePresentSrcKHR
		);

		// depth
		attachmentDescriptions[1] = vk::AttachmentDescription(
			{},
			DepthBuffer::format,
			vk::SampleCountFlagBits::e1,
			vk::AttachmentLoadOp::eClear,
			vk::AttachmentStoreOp::eDontCare,
			vk::AttachmentLoadOp::eDontCare,
			vk::AttachmentStoreOp::eDontCare,
			vk::ImageLayout::eUndefined,
			vk::ImageLayout::eDepthStencilAttachmentOptimal
		);

		vk::AttachmentReference colorReference(0, vk::ImageLayout::eColorAttachmentOptimal);
		vk::AttachmentReference depthReference(1, vk::ImageLayout::eDepthStencilAttachmentOptimal);

		vk::SubpassDescription subpassDescription(
			{},
			vk::PipelineBindPoint::eGraphics,
			0,
			nullptr,
			1,
			&colorReference,
			nullptr,
			&depthReference,
			0,
			nullptr
		);

		vk::RenderPassCreateInfo renderPassCreateInfo(
			{},
			2,
			attachmentDescriptions,
			1,
			&subpassDescription
		);

		renderPass = GetComponent<Device>().GetVulkanObject().createRenderPassUnique(renderPassCreateInfo);
	}

	void RendererImpl::InitShaders()
	{
		for (const auto& shaderInfo : ci.shaders) {
			CreateShader(shaderInfo);
			if (vertexShaderId.is_nil() && shaderInfo.type == ShaderType::Vertex) {
				EnableShader(shaderInfo.id);
			}
			if (fragmentShaderId.is_nil() && shaderInfo.type == ShaderType::Fragment) {
				EnableShader(shaderInfo.id);
			}
		}
		if (vertexShaderId.is_nil()) {
			SIS_THROW("Unable to find a vertex shader");
		}
		if (fragmentShaderId.is_nil()) {
			SIS_THROW("Unable to find a fragment shader");
		}
	}

	void RendererImpl::InitVertexBuffer(size_t size)
	{
		logger->BeginSection("Vertex Buffer");

		auto physicalDevice = GetComponent<PhysicalDevice>().GetVulkanObject();

		vertexBuffer = std::make_unique<VertexBuffer>(VertexBuffer::CreateInfo{
			size,
			GetComponent<Device>(),
			physicalDevice
		});

		logger->Log("Vertex Buffer of size " + std::to_string(size) + " initialized!");
		logger->EndSection();
	}

	void RendererImpl::InitPipeline(uint32_t stride)
	{
		SIS_DEBUGASSERT(pipelineLayout);
		SIS_DEBUGASSERT(renderPass);

		if (!ShaderExists(vertexShaderId)) {
			SIS_THROW("Vertex shader not found");
		}
		if (!ShaderExists(fragmentShaderId)) {
			SIS_THROW("Fragment shader not found");
		}

		vk::PipelineShaderStageCreateInfo shaderStageCreateInfos[2]{
			vk::PipelineShaderStageCreateInfo{
				{},
				vk::ShaderStageFlagBits::eVertex,
				GetShader(vertexShaderId).GetModule(),
				"main"
			},
			vk::PipelineShaderStageCreateInfo{
				{},
				vk::ShaderStageFlagBits::eFragment,
				GetShader(fragmentShaderId).GetModule(),
				"main"
			}
		};

		vk::VertexInputBindingDescription vertexInputBindingDescription(0, stride);
		vk::VertexInputAttributeDescription vertexInputAttributeDescription(0, 0, vk::Format::eR32G32B32Sfloat);
		vk::PipelineVertexInputStateCreateInfo pipelineVertexInputStateCreateInfo{
			{},
			1,
			&vertexInputBindingDescription,
			1,
			&vertexInputAttributeDescription
		};

		vk::PipelineInputAssemblyStateCreateInfo pipelineInputAssemblyStateCreateInfo{
			{},
			vk::PrimitiveTopology::eTriangleList
		};

		vk::PipelineViewportStateCreateInfo pipelineViewportStateCreateInfo{
			{},
			1,
			nullptr,
			1,
			nullptr
		};

		vk::PipelineRasterizationStateCreateInfo pipelineRasterizationStateCreateInfo{
			{},
			false,
			false,
			vk::PolygonMode::eFill,
			vk::CullModeFlagBits::eBack,
			vk::FrontFace::eClockwise,
			false,
			0.0f,
			0.0f,
			0.0f,
			1.0f
		};

		vk::PipelineMultisampleStateCreateInfo pipelineMultisampleStateCreateInfo;

		vk::PipelineDepthStencilStateCreateInfo pipelineDepthStencilStateCreateInfo{
			{},
			true,
			true,
			vk::CompareOp::eLessOrEqual
		};

		vk::ColorComponentFlags colorComponentFlags(vk::ColorComponentFlagBits::eR | vk::ColorComponentFlagBits::eG | vk::ColorComponentFlagBits::eB | vk::ColorComponentFlagBits::eA);
		vk::PipelineColorBlendAttachmentState pipelineColorBlendAttachmentState{
			false,
			vk::BlendFactor::eZero,
			vk::BlendFactor::eZero,
			vk::BlendOp::eAdd,
			vk::BlendFactor::eZero,
			vk::BlendFactor::eZero,
			vk::BlendOp::eAdd,
			colorComponentFlags
		};
		vk::PipelineColorBlendStateCreateInfo pipelineColorBlendStateCreateInfo{
			{},
			false,
			vk::LogicOp::eNoOp,
			1,
			&pipelineColorBlendAttachmentState,
			{{(1.0f, 1.0f, 1.0f, 1.0f)}}
		};

		std::vector<vk::DynamicState> dynamicStates{ vk::DynamicState::eViewport, vk::DynamicState::eScissor };
		vk::PipelineDynamicStateCreateInfo pipelineDynamicStateCreateInfo{
			{},
			static_cast<uint32_t>(dynamicStates.size()),
			dynamicStates.data()
		};

		vk::GraphicsPipelineCreateInfo graphicsPipelineCreateInfo{
			{},
			2,
			shaderStageCreateInfos,
			&pipelineVertexInputStateCreateInfo,
			&pipelineInputAssemblyStateCreateInfo,
			nullptr,
			&pipelineViewportStateCreateInfo,
			&pipelineRasterizationStateCreateInfo,
			&pipelineMultisampleStateCreateInfo,
			&pipelineDepthStencilStateCreateInfo,
			&pipelineColorBlendStateCreateInfo,
			&pipelineDynamicStateCreateInfo,
			*pipelineLayout,
			*renderPass
		};

		pipeline = GetComponent<Device>().GetVulkanObject().createGraphicsPipelineUnique(nullptr, graphicsPipelineCreateInfo);
	}

	Shader& RendererImpl::GetShader(uuids::uuid id)
	{
		if (!ShaderExists(id)) {
			SIS_THROW("Shader " + uuids::to_string(id) + " not found");
		}
		return *shaders[id];
	}

	void RendererImpl::CreateShader(const ShaderInfo& shaderInfo)
	{
		Shader::CreateInfo shaderCreateInfo{
			shaderInfo.code,
			shaderInfo.type,
			GetComponent<Device>()
		};
		auto shader = std::make_unique<Shader>(shaderCreateInfo);
		auto id = shaderInfo.id;
		shaders[id] = std::move(shader);

		logger->Log("Shader " + uuids::to_string(id) + " created!");
	}

	bool RendererImpl::ShaderExists(uuids::uuid id) const
	{
		return shaders.find(id) != shaders.end();
	}

	void RendererImpl::EnableShader(uuids::uuid id)
	{
		if (!ShaderExists(id)) {
			SIS_THROW("Shader " + uuids::to_string(id) + " does not exist");
		}
		auto type = shaders[id]->GetType();
		switch (type) {
		case ShaderType::Vertex:
			vertexShaderId = id;
			break;
		case ShaderType::Fragment:
			fragmentShaderId = id;
			break;
		default:
			SIS_DEBUGASSERT(false);
			break;
		}

		logger->Log("Shader " + uuids::to_string(id) + " enabled!");
	}

	void RendererImpl::UpdateUniformBuffer(Renderer::UniformBufferData data)
	{
		SIS_DEBUGASSERT(uniformBuffer);
		uniformBuffer->UpdateData(data);
	}

	Renderer::UniformBufferData RendererImpl::GetUniformBufferData()
	{
		SIS_DEBUGASSERT(uniformBuffer);
		return uniformBuffer->GetData<Renderer::UniformBufferData>();
	}

}