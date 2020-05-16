#include "AssetManagement/AssetReader.h"
#include "Utils/FilesystemUtils.h"
#include <iostream>
#include "Editor.h"
#include "EditorCLI.h"

int main() {
	using namespace Sisyphus::Editor;

	Editor editor;

	EditorCLI cli(editor);
	cli.Run();

	return 0;
}