#include "AssetManagement/AssetManager.h"
#include "Utils/FilesystemUtils.h"

int main() {
	Path projectPath = R"(C:\Teleports\teleports\sisyphus\Renderer\project)";
	Path assetDir = projectPath / "Assets";

	AssetManagement::AssetManager assetManager(assetDir);
	return 0;
}