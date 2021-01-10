#include "EditorCLI.h"
#include "Logger/Logger.h"

namespace Sisyphus::Editor {
	EditorCLI::EditorCLI(Editor& inEditor):
		editor(inEditor),
		exitFlag(false)
	{
		statusCmd = app.add_subcommand("status", "Shows current status");
		statusCmd->callback([this]() { Status(); });

		openProjectCmd = app.add_subcommand("open_project", "Opens a project");
		openProjectCmd->add_option("-p,--path", openProject_path);
		openProjectCmd->callback([this]() { OpenProject(); });

		packAssetsCmd = app.add_subcommand("pack_assets", "Packs assets");
		packAssetsCmd->callback([this]() { PackAssets(); });

		exitCmd = app.add_subcommand("exit");
		exitCmd->callback([this]() {exitFlag = true; });
	}

	void EditorCLI::Run()
	{
		std::cout << app.help();
		while (!exitFlag) {
			std::string cmd;
			std::getline(std::cin, cmd);
			try {
				app.parse(cmd);
			}
			catch (std::exception& e) {
				Logger().Log("Error processing command: " + cmd, LogLevel::Error);
				Logger().Log(e.what(), LogLevel::Error);
			}
		}
	}

	void EditorCLI::Status()
	{
		auto project = editor.CurrentProject();
		if (project)
		{
			std::cout << "Project: " << project->Name() << "\n";
		}
		else
		{
			std::cout << "No project is open\n";
		}
	}

	void EditorCLI::OpenProject()
	{
		editor.OpenProject(openProject_path);
	}

	void EditorCLI::PackAssets() {
		auto project = editor.CurrentProject();
		if (project)
		{
			project->PackAssets();
		}
		else
		{
			std::cout << "No project is open\n";
		}
	}
}