#include "Project.h"
#include "Utils/Throw.h"
#include "Filesystem/Filesystem.h"
#include "AssetManagement/AssetReader.h"

namespace Sisyphus::Editor {
	namespace Am = AssetManagement;

	Project::Project(const Fs::Path& inPath) :
		path(inPath)
	{
		SIS_THROWASSERT(Fs::IsDirectory(path));
		name = path.LastSegment();
		auto projectInfoPath = path / (name + ".projectInfo.json");
		SIS_THROWASSERT(Fs::Exists(projectInfoPath));

		auto assetDir = path / "assets/";
		auto assetReader = Am::AssetReader::Create(Am::AssetReader::ReaderType::Unpacked);
		assetReader->ReadAssets(assetDir.String());
	}

	std::string Project::Name() const {
		return name;
	}
}