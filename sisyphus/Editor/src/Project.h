#pragma once
#include <string>
#include "Filesystem/Path.h"

namespace Sisyphus::Editor {

	class Project {
	public:
		Project(const Fs::Path& path);

		std::string Name() const;

	private:
		std::string name;
		Fs::Path path;
	};
}