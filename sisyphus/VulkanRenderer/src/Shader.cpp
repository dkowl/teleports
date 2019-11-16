#include "Shader.h"
#include "SPIRV/GlslangToSpv.h"
#include "glslang/StandAlone/ResourceLimits.h"
#include "Utils/Logger.h"
#include <vector>

namespace Sisyphus::Rendering::Vulkan {
	EShLanguage ShaderTypeToEShLanguage(ShaderType type) {
		switch (type) {
		case ShaderType::Vertex: return EShLangVertex;
		case ShaderType::Fragment: return EShLangFragment;
		default: throw(std::runtime_error("Unknown shader type"));
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
				throw std::runtime_error("Shared parsing failed");
			}
			
			program.addShader(&shader);
			bool linkSuccess = program.link(messages);
			if (!linkSuccess) {
				throw std::runtime_error("Shader program linking failed");
			}
		}
		catch (std::runtime_error& e) {
			Logger::Get().Log(shader.getInfoLog());
			Logger::Get().Log(shader.getInfoDebugLog());
			throw e;
		}

		std::vector<unsigned int> result;
		glslang::GlslangToSpv(*program.getIntermediate(eshLang), result);
		return result;
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
	}
	ShaderType Shader::GetType() const
	{
		return shaderType;
	}
	vk::ShaderModule Shader::GetModule() const
	{
		return *shaderModule;
	}
}