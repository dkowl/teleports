#include "Editor.h"
#include "Filesystem/Path.h"
#include <filesystem>
#include "Logger/Logger.h"

namespace Sisyphus::Editor {
	namespace {
		Fs::Path SavePath() {
			Fs::Path p = std::filesystem::temp_directory_path().string();
			p /= "Local";
			p /= "Sisyphus";
			p /= "editorState.json";

			return p;
		}
	}

	Editor::Editor() {
		ReadState();

		const auto& lastProjectPaths = state.LastOpenedProjects();
		if (lastProjectPaths.empty()) {
			Logger().Log("Editor: No recent projects found", LogLevel::Info);
		}
		else {
			Logger().BeginSection("Editor: Recent projects:");
			for (auto&& p : lastProjectPaths) {
				Logger().Log(p.String(), LogLevel::Info);
			}
		}
	}

	Editor::~Editor() {
		Logger().Log("Editor: Saving state", LogLevel::Info);
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
	void Editor::CloseCurrentProject()
	{
		currentProject = std::nullopt;
	}
}
