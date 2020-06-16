#include "Project.h"
#include "Utils/Throw.h"
#include "Filesystem/Filesystem.h"
#include "AssetManagement/AssetReader.h"
#include "Logger/Logger.h"

namespace Sisyphus::Editor {
	namespace Am = AssetManagement;

	Project::Project(const Fs::Path& inPath) :
		path(inPath)
	{
		Logger().Log("Opening project: " + inPath.String(), LogLevel::Info);
		SIS_THROWASSERT(Fs::IsDirectory(path));
		name = path.LastSegment();
		auto projectInfoPath = path / (name + ".projectInfo.json");
		SIS_THROWASSERT(Fs::Exists(projectInfoPath));

		auto assetDir = path / "assets/";
		auto assetReader = Am::AssetReader::Create(Am::AssetReaderType::Unpacked);
		assetReader->Read(assetDir.String());
	}

	std::string Project::Name() const {
		return name;
	}
}