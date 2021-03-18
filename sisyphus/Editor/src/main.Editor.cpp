#include "AssetManagement/AssetReader.h"
#include "Utils/FilesystemUtils.h"
#include <iostream>
#include "Editor.h"
#include "EditorCLI.h"

int main() {
	using namespace Sisyphus::Editor;

	try {
		Editor editor;

		EditorCLI cli(editor);
		cli.Run();
	}
	catch (std::runtime_error& e) {
		std::cerr << e.what() << "\n";
	}
	catch (...) {
		std::cerr << "Unknown exception.\n";
	}

	return 0;
}