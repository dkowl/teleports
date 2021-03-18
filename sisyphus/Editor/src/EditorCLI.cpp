#include "EditorCLI.h"
#include "Logger/Logger.h"

namespace Sisyphus::Editor {
	EditorCLI::EditorCLI(Editor& inEditor):
		out(std::cout),
		editor(inEditor),
		exitFlag(false)
	{
		helpCmd = app.add_subcommand("help", "Shows help");
		helpCmd->callback([this]() {Help(); });

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
		Help();
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

	void EditorCLI::Help()
	{
		out << app.help("", CLI::AppFormatMode::All);
	}

	void EditorCLI::Status()
	{
		auto project = editor.CurrentProject();
		if (project)
		{
			out << "Project: " << project->Name() << "\n";
		}
		else
		{
			out << "No project is open\n";
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
			out << "No project is open\n";
		}
	}
}