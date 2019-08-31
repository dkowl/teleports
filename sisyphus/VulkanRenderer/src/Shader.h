#pragma once

#include "Vulkan.h"
#include "ShaderType.h"
#include <string>

namespace Vulkan {
	class Shader {
	public:
		struct CreateInfo {
			std::string glslCode;
			ShaderType shaderType;
			vk::Device device;
		};

		Shader(const CreateInfo & ci);

	private:
		ShaderType shaderType;
		vk::UniqueShaderModule shaderModule;
	};
}