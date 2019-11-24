#pragma once
#include "Window.h"

namespace WindowCreator {
	class WindowsWindow : public Window {

	public:
		struct CreateInfo {
			uint32_t width;
			uint32_t height;
		};

		WindowsWindow(CreateInfo ci);
		~WindowsWindow();

		std::optional<WindowEvent> GetEvent() override;
		vk::UniqueSurfaceKHR GetVulkanSurface(vk::Instance instance) override;

	private:

		struct PrivateData;
		std::unique_ptr<PrivateData> data;
	};
}