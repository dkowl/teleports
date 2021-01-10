#pragma once
#include <string>
#include <memory>
#include "Filesystem/Path.h"

namespace Sisyphus::Editor {

	class Project {
	public:
		Project(const Fs::Path& path);
		~Project();

		std::string Name() const;

		void PackAssets();

	private:
		std::string name;
		Fs::Path path;
		struct PrivateData;
		std::unique_ptr<PrivateData> impl;
	};
}