#pragma once
#include "Editor.h"
#include "CLI11.hpp"
#include <ostream>

namespace Sisyphus::Editor {
	class EditorCLI {
	public:
		EditorCLI(Editor& inEditor);

		void Run();

		void Help();
		void Status();
		void OpenProject();
		void PackAssets();

	private:
		Editor& editor;

		std::ostream& out;

		CLI::App app;

		CLI::App* helpCmd;

		CLI::App* statusCmd;

		CLI::App* openProjectCmd;
		std::string openProject_path;

		CLI::App* packAssetsCmd;

		CLI::App* exitCmd;
		bool exitFlag;
	};
}