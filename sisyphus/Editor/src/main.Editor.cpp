#include "AssetManagement/AssetReader.h"
#include "Utils/FilesystemUtils.h"
#include <iostream>
#include "CLI11.hpp"
#include "Editor.h"

int main() {
	using namespace Sisyphus::Editor;

	Editor editor;
	editor.OpenProject(R"(C:\Repos\teleports\sisyphus\Projects\TestProject)");

	return 0;
}