#pragma once
#include "Editor.h"
#include "CLI11.hpp"

namespace Sisyphus::Editor {
	class EditorCLI {
	public:
		EditorCLI(Editor& inEditor);

		void Run();

		void Status();
		void OpenProject();
		void PackAssets();

	private:
		Editor& editor;

		CLI::App app;

		CLI::App* statusCmd;

		CLI::App* openProjectCmd;
		std::string openProject_path;

		CLI::App* packAssetsCmd;

		CLI::App* exitCmd;
		bool exitFlag;
	};
}