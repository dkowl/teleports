#include "Editor.h"
#include "Filesystem/Path.h"
#include <filesystem>
#include "Logger/Logger.h"

namespace Sisyphus::Editor {
	namespace {
		Fs::Path SavePath() {
			Fs::Path p = std::filesystem::temp_directory_path().parent_path().string();
			p /= "Sisyphus";
			p /= "editorState.json";

			return p;
		}
	}

	Editor::Editor() {
		ReadState();

		const auto& lastProjectPaths = state.LastOpenedProjects();
		if (lastProjectPaths.empty()) {
			Logger().Log("No recent projects found", LogLevel::Info);
		}
		else {
			Logger().BeginSection("Recent projects:");
			for (auto&& p : lastProjectPaths) {
				Logger().Log(p.String(), LogLevel::Info);
			}
			Logger().EndSection();
		}

		OpenMostRecentProject();
	}

	Editor::~Editor() {
		SaveState();
	}

	void Editor::ReadState()
	{
		state.ReadFromFile(SavePath());		
	}
	void Editor::SaveState()
	{
		state.SaveToFile(SavePath());
	}
	void Editor::OpenProject(const Fs::Path& path)
	{
		CloseCurrentProject();
		currentProject = Project(path);
		state.OnProjectOpened(path);
	}
	void Editor::OpenMostRecentProject()
	{
		for (auto&& path : state.LastOpenedProjects()) {
			try {
				OpenProject(path);
			}
			catch (...) {
				Logger().Log("Failed to open recent project: " + path.String(), LogLevel::Warn);
				Logger().Log("Removing the project from the recent list", LogLevel::Warn);
				state.PopLastOpenedProject();
				continue;
			}
			break;
		}
	}
	void Editor::CloseCurrentProject()
	{
		currentProject = std::nullopt;
	}

	Project* Editor::CurrentProject()
	{
		if (currentProject)
		{
			return &(*currentProject);
		}
		else
		{
			return nullptr;
		}
	}
}
