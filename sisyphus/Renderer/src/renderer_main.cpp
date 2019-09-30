#include <exception>
#include <iostream>
#include "VulkanRenderer\Renderer.h"
#include "Utils\Logger.h"
#include "AssetManagement/AssetManager.h"

int main() {
	try {
		AssetManagement::AssetManager assetManager("Assets");

		auto logger = &Logger::Get();
		Vulkan::Renderer::CreateInfo rendererCreateInfo;
		rendererCreateInfo.windowWidth = 1280;
		rendererCreateInfo.windowHeight = 720;
		rendererCreateInfo.logger = logger;

		logger->BeginSection("Vulkan Renderer");
		Vulkan::Renderer renderer(rendererCreateInfo);

		Vulkan::Renderer::UniformBufferData ubData{ 255, 0, 0 };
		renderer.UpdateUniformBuffer(ubData);
		logger->EndSection();
		logger->Log("Uniform Buffer updated with 255, 0, 0\n");

		auto ubDataFromGPU = renderer.GetUniformBufferData();
		if (memcmp(&ubData, &ubDataFromGPU, sizeof(ubData)) != 0) {
			throw std::logic_error("Uniform buffer data manipulation error!");
		}

		auto vertexShaderFileId = uuids::uuid::from_string("e1124008-e112-4008-a2f3-cf6233498020").value();
		String vertexShaderText(assetManager.GetAsset(vertexShaderFileId).GetDataAsString());
		auto fragmentShaderFileId = uuids::uuid::from_string("ce637e01-1d00-405c-8aaa-f0c022235745").value();
		String fragmentShaderText(assetManager.GetAsset(fragmentShaderFileId).GetDataAsString());

		renderer.CreateShader(
			uuids::uuid::from_string("9b8c0852-be27-4c0e-add9-da8e2ccf464f").value(), 
			vertexShaderText, 
			ShaderType::Vertex);

		renderer.CreateShader(
			uuids::uuid::from_string("36b455c9-b2e0-4bae-a89a-8f7fc750ff74").value(),
			fragmentShaderText,
			ShaderType::Fragment);
	}
	catch (std::exception& e) {
		std::cout << e.what() << std::endl;
		system("PAUSE");
	}
}