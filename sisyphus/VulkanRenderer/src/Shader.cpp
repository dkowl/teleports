#include "Pch_VulkanRenderer.h"
#include "Shader.h"
#include "SPIRV/GlslangToSpv.h"
#include "glslang/StandAlone/ResourceLimits.h"
#include "Logger/Logger.h"
#include "Utils/Throw.h"
#include "Renderer/VertexInputLayout.h"

namespace Sisyphus::Rendering::Vulkan {
	namespace {
		EShLanguage ShaderTypeToEShLanguage(ShaderType type) {
			switch (type) {
			case ShaderType::Vertex: return EShLangVertex;
			case ShaderType::Fragment: return EShLangFragment;
			default: { SIS_THROW("Unknown shader type"); return EShLangVertex; }
			}
		}

		std::vector<unsigned int> GlslToSpv(const std::string& glslCode, ShaderType shaderType) {
			auto eshLang = ShaderTypeToEShLanguage(shaderType);

			const char* string = glslCode.data();
			glslang::TShader shader(eshLang);
			glslang::TProgram program;
			shader.setStrings(&string, 1);

			EShMessages messages = (EShMessages)(EShMsgSpvRules | EShMsgVulkanRules);

			try {
				bool parseSuccess = shader.parse(&glslang::DefaultTBuiltInResource, 100, false, messages);
				if (!parseSuccess) {
					SIS_THROW("Shared parsing failed");
				}

				program.addShader(&shader);
				bool linkSuccess = program.link(messages);
				if (!linkSuccess) {
					SIS_THROW("Shader program linking failed");
				}
			}
			catch (std::runtime_error& e) {
				Logger().Log(shader.getInfoLog());
				Logger().Log(shader.getInfoDebugLog());
				throw e;
			}

			std::vector<unsigned int> result;
			glslang::GlslangToSpv(*program.getIntermediate(eshLang), result);
			return result;
		}		

		VulkanVertexInputLayout VertexInputLayoutToVulkan(const VertexInputLayout& layout) {
			VulkanVertexInputLayout result;
			uint32_t currentLocation = 0;
			for (uint32_t i = 0; i < layout.bindings.size(); i++) {
				const auto& binding = layout.bindings[i];
				result.bindings.push_back({
					i,
					binding.stride
				});

				for (int j = 0; j < binding.attributes.size(); j++) {
					const auto& attribute = binding.attributes[j];
					bool allFloat = true;
					bool all32bit = true;
					int numComponents = static_cast<int>(attribute.components.size());
					int totalBits = 0;
					for (auto&& component : attribute.components) {
						if (component.first != ComponentType::Float) {
							allFloat = false;
						}
						if (component.second != 32) {
							all32bit = false;
						}
						totalBits += component.second;
					}

					vk::Format vulkanFormat = vk::Format::eUndefined;
					if (allFloat && all32bit && numComponents == 3) {
						vulkanFormat = vk::Format::eR32G32B32Sfloat;
					}
					// TODO: Recognize more formats
					if (vulkanFormat == vk::Format::eUndefined) {
						SIS_THROW("Unrecognized Vulkan format");
					}

					result.attributes.push_back({
						currentLocation,
						i,
						vulkanFormat,
						attribute.offset
					});

					currentLocation += ((totalBits - 1) / 64) + 1;
				}
			}

			result.pipelineVertextInputStateCreateInfo = vk::PipelineVertexInputStateCreateInfo{
				{},
				uint32_t(result.bindings.size()),
				result.bindings.data(),
				uint32_t(result.attributes.size()),
				result.attributes.data()
			};

			return result;
		}
	}

	Shader::Shader(const CreateInfo & ci):
		shaderType(ci.shaderType)
	{
		static bool glslangInitialized = false;
		if (!glslangInitialized) {
			glslang::InitializeProcess();
			glslangInitialized = true;
		}

		std::vector<unsigned int> spirv = GlslToSpv(ci.glslCode, ci.shaderType);
		vk::ShaderModuleCreateInfo moduleCreateInfo(
			{},
			spirv.size() * sizeof(unsigned int),
			spirv.data()
		);

		shaderModule = ci.device.createShaderModuleUnique(moduleCreateInfo);
		if (ci.vertexInputLayout) {
			vulkanVertexInputLayout = VertexInputLayoutToVulkan(*ci.vertexInputLayout);
		}
		else if (shaderType == ShaderType::Vertex) {
			SIS_THROW("Vertex input layout not found in a vertex shader");
		}
	}
	ShaderType Shader::GetType() const
	{
		return shaderType;
	}
	vk::ShaderModule Shader::GetModule() const
	{
		return *shaderModule;
	}
	std::optional<vk::PipelineVertexInputStateCreateInfo> Shader::GetPipelineVertexInputStateCreateInfo() const
	{
		if (vulkanVertexInputLayout) {
			return vulkanVertexInputLayout->pipelineVertextInputStateCreateInfo;
		}
		else {
			return std::nullopt;
		}
	}
}