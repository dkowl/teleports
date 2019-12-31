#include "AssetManagement/AssetManager.h"
#include "Utils/FilesystemUtils.h"

int main() {
	using namespace Sisyphus;

	Path projectPath = R"(C:\Teleports\teleports\sisyphus\Renderer\project)";
	Path assetDir = projectPath / "Assets";

	AssetManagement::AssetManager assetManager(assetDir.string());
	return 0;
}