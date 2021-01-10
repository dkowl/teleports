#include "Project.h"
#include "Utils/Throw.h"
#include "Filesystem/Filesystem.h"
#include "AssetManagement/AssetReader.h"
#include "AssetManagement/AssetPacker.h"
#include "Logger/Logger.h"

namespace Sisyphus::Editor {
	namespace Am = AssetManagement;

	struct Project::PrivateData
	{
		std::unique_ptr<Am::AssetReader> assetReader;
	};

	Project::Project(const Fs::Path& inPath) :
		path(inPath)
	{
		Logger().Log("Opening project: " + inPath.String(), LogLevel::Info);
		SIS_THROWASSERT(Fs::IsDirectory(path));
		name = path.LastSegment();
		auto projectInfoPath = path / (name + ".projectInfo.json");
		SIS_THROWASSERT(Fs::Exists(projectInfoPath));

		auto assetDir = path / "assets/";
		impl = std::make_unique<PrivateData>();
		impl->assetReader = Am::AssetReader::Create(Am::AssetReaderType::Unpacked);
		impl->assetReader->Read(assetDir.String());
	}

	Project::~Project() = default;

	std::string Project::Name() const {
		return name;
	}

	void Project::PackAssets()
	{
		Am::AssetPacker packer;
		packer.PackAssets(*impl->assetReader, path / "assets_packed");
	}
}