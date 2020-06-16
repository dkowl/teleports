#include <iostream>
#include <unordered_map>
#include "AssetManagement/AssetReader.h"
#include "Renderer/RendererFactory.h"
#include "Renderer/ShaderInfo.h"
#include "WindowCreator/WindowCreator.h"

using namespace Sisyphus;
using namespace AssetManagement;
using namespace Rendering;
namespace wc = WindowCreator;

struct Vertex {
	float x;
	float y;
	float z;
};

struct Model {
	std::vector<Vertex> vertices;
};

class Resources {
public:
	void Init() {
		assetReader = AssetReader::Create(AssetReaderType::Unpacked);
		assetReader->Read("assets");
	}

	ShaderInfo ReadShader(std::string idStr, ShaderType shaderType, std::optional<VertexInputLayout> layout = std::nullopt) {
		auto id = *uuids::uuid::from_string(idStr);
		auto shaderText = assetReader->GetAsset(id).Data().AsString();
		return ShaderInfo{ id, shaderText, shaderType, layout };
	}

private:
	std::unique_ptr<AssetReader> assetReader;
	std::unordered_map<uuids::uuid, Model> models;
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

		VertexInputAttribute positionAttr{ 
			{
				{ComponentType::Float, 32},
				{ComponentType::Float, 32},
				{ComponentType::Float, 32}
			},
			0
		};

		VertexInputBinding vertexBinding{
			{
				{positionAttr}
			},
			int(sizeof(Vertex))
		};

		VertexInputLayout vertexLayout{ 
			{ vertexBinding	} 
		};

		RendererCreateInfo rendererInfo;
		rendererInfo.type = RendererType::Vulkan;
		rendererInfo.window = window.get();
		rendererInfo.shaders = {
			resources.ReadShader("4b55f85e-9a60-4637-9d3e-a9b671c3189f", ShaderType::Vertex, vertexLayout),
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