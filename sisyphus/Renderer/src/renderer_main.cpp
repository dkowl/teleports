#include <vulkan/vulkan.hpp>
#include <exception>
#include <iostream>

int main() {
	try {
		vk::ApplicationInfo applicationInfo(
			"Vulkan App",
			VK_MAKE_VERSION(1, 0, 0),
			"Sisyphus",
			VK_MAKE_VERSION(1, 0, 0),
			VK_API_VERSION_1_0
		);

		vk::InstanceCreateInfo instanceCreateInfo(
			{},
			&applicationInfo
		);

		auto instance = vk::createInstanceUnique(instanceCreateInfo);
	}
	catch (std::exception& e) {
		std::cout << e.what() << std::endl;
	}
}