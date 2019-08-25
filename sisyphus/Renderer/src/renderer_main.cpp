#include <exception>
#include <iostream>
#include "VulkanRenderer\Renderer.h"
#include "Utils\OstreamLogger.h"

int main() {
	try {
		std::unique_ptr<ILogger> logger = std::make_unique<OstreamLogger>();
		Vulkan::Renderer::CreateInfo rendererCreateInfo;
		rendererCreateInfo.windowWidth = 1280;
		rendererCreateInfo.windowHeight = 720;
		rendererCreateInfo.logger = logger.get();

		logger->BeginSection("Vulkan Renderer");
		Vulkan::Renderer renderer(rendererCreateInfo);

		renderer.UpdateUniformBuffer({ '\255', '\0', '\0' });
		logger->EndSection();
		logger->Log("Uniform Buffer updated with 255, 0, 0\n");
	}
	catch (std::exception& e) {
		std::cout << e.what() << std::endl;
		system("PAUSE");
	}
}