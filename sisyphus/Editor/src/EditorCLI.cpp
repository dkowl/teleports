#include "EditorCLI.h"
#include <vector>
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

		InitRelease();

		exitCmd = app.add_subcommand("exit");
		exitCmd->callback([this]() {exitFlag = true; });
	}

	void EditorCLI::InitRelease()
	{
		releaseCmd = app.add_subcommand("release", "Builds a releasable version of the project");

		std::string description;
		const auto& platforms = editor.AvailablePlatforms();
		for (auto&& platform : platforms) {
			description += std::string(PlatformAsString(platform)) + " | ";
		}
		description += "All";
		releaseCmd->add_option("-p,--platform", release_platform, description);
		releaseCmd->callback([this]() { Release(); });
	}

	void EditorCLI::Run()
	{
		Help();
		while (!exitFlag) {
			std::string cmd;
			std::cout << (editor.CurrentProject() ? editor.CurrentProject()->Name() : "-no project-") << ":";
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

	void EditorCLI::Release()
	{
		auto project = editor.CurrentProject();
		if (!project) {
			out << "No project is open\n";
		}
		else {
			Project::ReleaseOptions options;
			options.platform = PlatformFromString(release_platform);

			project->Release(options);
		}
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