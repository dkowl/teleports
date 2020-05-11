#include "EditorState.h"
#include <fstream>
#include "Logger/Logger.h"
#include "Utils/Json.h"
#include "AssetManagement/ResourceLoader.h"
#include "AssetManagement/RawDataView.h"
#include "Filesystem/Filesystem.h"

namespace nlohmann {
	template<>
	struct adl_serializer<Sisyphus::Fs::Path> {
		static void to_json(json& j, const Sisyphus::Fs::Path& path) {
			j["path"] = path.String();
		}
		static void from_json(const json& j, Sisyphus::Fs::Path& path) {
			path = j["path"].get<std::string>();
		}
	};
}

namespace Sisyphus::Editor {
	const std::vector<Fs::Path> EditorState::LastOpenedProjects() const
	{
		return lastOpenedProjects;
	}
	void EditorState::SaveToFile(const Fs::Path& path) const {
		Fs::CreateDirectories(path);
		std::ofstream file(path.String());

		json j;
		j["lastOpenedProjects"] = lastOpenedProjects;

		file << j.dump();
		Logger().Log("Editor state saved to file");
}

	void EditorState::ReadFromFile(const Fs::Path& path) {
		try {
			RawData data;
			auto loadResult = AssetManagement::ResourceLoader::Load(data, path.String());
			
			if (!loadResult.ok) {
				Logger().Log("Saved editor state not found", LogLevel::Info);
				return;
			}

			RawDataView dataView(data);
			json j = json::parse(dataView.AsString());
			lastOpenedProjects = j["lastOpenedProjects"].get<std::vector<Fs::Path>>();
			Logger().Log("Editor state read from file", LogLevel::Info);
		}
		catch (...) {
			Logger().Log("Failed to read saved editor state", LogLevel::Info);
			lastOpenedProjects.clear();
		}
	}
	void EditorState::OnProjectOpened(const Fs::Path& path)
	{
		int found = 0;
		for (int i = 0; i < lastOpenedProjects.size() - found; i++) {			
			if (lastOpenedProjects[i] == path) {
				found++;
			}
			if (found) {
				lastOpenedProjects[i] = lastOpenedProjects[i + found];
			}
		}
		if (found) {
			lastOpenedProjects.erase(lastOpenedProjects.end() - found, lastOpenedProjects.end());
		}
		lastOpenedProjects.push_back(path);
	}
}