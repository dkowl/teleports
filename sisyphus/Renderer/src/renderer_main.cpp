#include <exception>
#include <iostream>
#include "VulkanRenderer\Renderer.h"

int main() {
	try {
		Vulkan::Renderer renderer({ 1280, 720 });
	}
	catch (std::exception& e) {
		std::cout << e.what() << std::endl;
		system("PAUSE");
	}
}