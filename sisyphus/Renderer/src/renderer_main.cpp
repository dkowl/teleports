#include "PlatformSpecific.h"
#include <exception>
#include <iostream>
#include "VulkanRenderer.h"

int main() {
	try {
		VulkanRenderer renderer({ 1280, 720 });
	}
	catch (std::exception& e) {
		std::cout << e.what() << std::endl;
	}
}