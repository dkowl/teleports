#pragma once
#include "Window.h"

namespace WindowCreator {
	class WindowsWindow : public Window {

	public:
		WindowsWindow();
		~WindowsWindow();

		void HandleEvent(WindowEvent event) override;
		vk::SurfaceKHR GetVulkanSurface(vk::Instance instance) override;

	private:

		struct PrivateData;
		std::unique_ptr<PrivateData> data;
	};
}