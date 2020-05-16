#pragma once
#include "Editor.h"
#include "CLI11.hpp"

namespace Sisyphus::Editor {
	class EditorCLI {
	public:
		EditorCLI(Editor& inEditor);

		void Run();

		void OpenProject();
		void PackAssets();

	private:
		Editor& editor;

		CLI::App app;
		std::string openProject_path;
		CLI::App* openProjectCmd;
		CLI::App* packAssetsCmd;
		bool exitFlag;
		CLI::App* exitCmd;
	};
}