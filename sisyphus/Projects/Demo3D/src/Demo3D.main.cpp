#include <iostream>
#include "AssetManagement/AssetReader.h"
#include "Renderer/RendererFactory.h"
#include "WindowCreator/WindowCreator.h"

using namespace Sisyphus;
using namespace AssetManagement;
using namespace Rendering;
namespace wc = WindowCreator;

class Resources {
public:
	void Init() {
		assetReader = AssetReader::Create(AssetReaderType::Unpacked);
		assetReader->Read("assets");
	}

	ShaderInfo ReadShader(std::string idStr, ShaderType shaderType) {
		auto id = *uuids::uuid::from_string(idStr);
		auto shaderText = assetReader->GetAsset(id).Data().AsString();
		return ShaderInfo{ id, shaderText, shaderType };
	}

private:
	std::unique_ptr<AssetReader> assetReader;
};

int main() {

	try {

		wc::WindowCreator windowCreator;
		auto window = windowCreator.Create(wc::WindowCreateInfo{
			wc::Platform::Windows,
			{1280, 720},
			});

		Resources resources;
		resources.Init();

		RendererCreateInfo rendererInfo;
		rendererInfo.type = RendererType::Vulkan;
		rendererInfo.window = window.get();
		rendererInfo.shaders = {
			resources.ReadShader("4b55f85e-9a60-4637-9d3e-a9b671c3189f", ShaderType::Vertex),
			resources.ReadShader("7cf15433-0729-4c9c-b147-8f05136a76c5", ShaderType::Fragment)
		};
		auto renderer = RendererFactory().Create(rendererInfo);

		while (true) {
			bool close = false;
			std::optional<wc::WindowEvent> windowEvent;
			while (true) {
				windowEvent = window->GetEvent();
				if (!windowEvent.has_value()) {
					break;
				}
				else if (windowEvent.value().type == wc::WindowEvent::Type::Close) {
					close = true;
					break;
				}
			}
			if (close) {
				break;
			}

		}
	}
	catch (std::exception& e) {
		std::cout << e.what() << std::endl;
	}
}