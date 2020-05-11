#include "Project.h"
#include "Utils/Throw.h"
#include "Filesystem/Filesystem.h"

namespace Sisyphus::Editor {
	Project::Project(const Fs::Path& inPath) :
		path(inPath)
	{
		SIS_THROWASSERT(Fs::IsDirectory(path));
		name = path.LastSegment();
		auto projectInfoPath = path / (name + ".projectInfo.json");
		SIS_THROWASSERT(Fs::Exists(projectInfoPath));
	}

	std::string Project::Name() const {
		return name;
	}
}