#pragma once
#include "Window.h"

namespace Sisyphus::WindowCreator {
	class WindowsWindow : public Window {

	public:
		struct CreateInfo {
			WindowExtent extent;
		};

		WindowsWindow(CreateInfo ci);
		~WindowsWindow();

		WindowExtent GetExtent() const override;
		std::optional<WindowEvent> GetEvent() override;
		vk::UniqueSurfaceKHR GetVulkanSurface(vk::Instance instance) override;

	private:

		struct PrivateData;
		std::unique_ptr<PrivateData> data;
	};
}