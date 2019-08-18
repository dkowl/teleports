#include <exception>
#include <iostream>
#include "VulkanRenderer\Renderer.h"

int main() {
	try {
		Vulkan::Renderer renderer({ 1280, 720 });

		renderer.UpdateUniformBuffer({ '\255', '\0', '\0' });
		std::cout << "Uniform Buffer updated with 255, 0, 0\n\n";
	}
	catch (std::exception& e) {
		std::cout << e.what() << std::endl;
		system("PAUSE");
	}
}