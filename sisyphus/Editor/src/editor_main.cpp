#include "AssetManagement/AssetReader.h"
#include "Utils/FilesystemUtils.h"
#include <iostream>
#include "CLI11.hpp"

int main() {
	using namespace Sisyphus;

	Path projectPath = R"(C:\Repos\teleports\sisyphus\Projects\TestProject)";
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