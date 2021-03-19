#pragma once
#include <memory>
#include <unordered_map>
#include <vector>
#include "Utils/Platform.h"
#include "Project.h"
#include "EditorState.h"

namespace Sisyphus::Editor {

	class Editor {
	public:
		Editor();
		Editor(const Editor&) = delete;
		Editor(Editor&&) = delete;
		~Editor();
		void operator=(const Editor&) = delete;
		void operator=(Editor&&) = delete;

		void ReadState();
		void SaveState();

		void OpenProject(const Fs::Path& path);
		void OpenMostRecentProject();
		void CloseCurrentProject();

		Project* CurrentProject();
		const std::vector<Platform>& AvailablePlatforms();

	private:
		std::unique_ptr<Project> currentProject;
		EditorState state;

		std::vector<Platform> platforms;
	};
}