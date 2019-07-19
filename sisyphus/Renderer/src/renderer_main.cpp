#include "PlatformSpecific.h"
#include <exception>
#include <iostream>
#include "VulkanRenderer.h"

int main() {
	try {
		VulkanRenderer renderer;
	}
	catch (std::exception& e) {
		std::cout << e.what() << std::endl;
	}
}