#include <exception>
#include <iostream>
#include "VulkanRenderer\Renderer.h"
#include "Utils\Logger.h"

const std::string vertexShaderText_PC_C = R"(
#version 400
#extension GL_ARB_separate_shader_objects : enable
#extension GL_ARB_shading_language_420pack : enable
layout (std140, binding = 0) uniform buffer
{
  mat4 mvp;
} uniformBuffer;
layout (location = 0) in vec4 pos;
layout (location = 1) in vec4 inColor;
layout (location = 0) out vec4 outColor;
void main()
{
  outColor = inColor;
  gl_Position = uniformBuffer.mvp * pos;
}
)";

const std::string fragmentShaderText_C_C = R"(
#version 400
#extension GL_ARB_separate_shader_objects : enable
#extension GL_ARB_shading_language_420pack : enable
layout (location = 0) in vec4 color;
layout (location = 0) out vec4 outColor;
void main()
{
  outColor = color;
}
)";

int main() {
	try {
		auto logger = &Logger::Get();
		Vulkan::Renderer::CreateInfo rendererCreateInfo;
		rendererCreateInfo.windowWidth = 1280;
		rendererCreateInfo.windowHeight = 720;
		rendererCreateInfo.logger = logger;

		logger->BeginSection("Vulkan Renderer");
		Vulkan::Renderer renderer(rendererCreateInfo);

		renderer.UpdateUniformBuffer({ '\255', '\0', '\0' });
		logger->EndSection();
		logger->Log("Uniform Buffer updated with 255, 0, 0\n");

		renderer.CreateShader(
			uuids::uuid::from_string("9b8c0852-be27-4c0e-add9-da8e2ccf464f").value(), 
			vertexShaderText_PC_C, 
			ShaderType::Vertex);

		renderer.CreateShader(
			uuids::uuid::from_string("36b455c9-b2e0-4bae-a89a-8f7fc750ff74").value(),
			fragmentShaderText_C_C,
			ShaderType::Fragment);
	}
	catch (std::exception& e) {
		std::cout << e.what() << std::endl;
		system("PAUSE");
	}
}