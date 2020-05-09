#include "AssetManagement/AssetReader.h"
#include "Utils/FilesystemUtils.h"
#include <iostream>

int main() {
	using namespace Sisyphus;

	Path projectPath = R"(C:\Repos\teleports\sisyphus\Projects\TestProject\Windows\project)";
	Path assetDir = projectPath / "Assets";

	auto assetReader = AssetManagement::AssetReader::Create(AssetManagement::AssetReader::ReaderType::Unpacked);
	try {
		assetReader->ReadAssets(assetDir.string());
	}
	catch (std::exception& e) {
		std::cout << e.what() << std::endl;
	}
	return 0;
}