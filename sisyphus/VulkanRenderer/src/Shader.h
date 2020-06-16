#pragma once

#include "Vulkan.h"
#include "Renderer\VertexInputLayout.h"
#include "Renderer\ShaderType.h"
#include <string>
#include <optional>

namespace Sisyphus::Rendering::Vulkan {
	struct VulkanVertexInputLayout {
		std::vector<vk::VertexInputBindingDescription> bindings;
		std::vector<vk::VertexInputAttributeDescription> attributes;
		vk::PipelineVertexInputStateCreateInfo pipelineVertextInputStateCreateInfo;
	};

	class Shader {
	public:
		struct CreateInfo {
			std::string glslCode;
			ShaderType shaderType;			
			vk::Device device;
			std::optional<VertexInputLayout> vertexInputLayout;
		};

		Shader(const CreateInfo & ci);

		ShaderType GetType() const;
		vk::ShaderModule GetModule() const;
		std::optional<vk::PipelineVertexInputStateCreateInfo> GetPipelineVertexInputStateCreateInfo() const;

	private:
		ShaderType shaderType;
		vk::UniqueShaderModule shaderModule;
		std::optional<VulkanVertexInputLayout> vulkanVertexInputLayout;
	};
}