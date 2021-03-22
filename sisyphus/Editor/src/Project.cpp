#include "Project.h"
#include "Utils/SimpleTimer.h"
#include "Utils/TimeUtils.h"
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

	void CreateDirIfNotExists(const Fs::Path& p) {
		if (!Fs::Exists(p)) {
			Logger().Log("Creating directory " + p.String());
			Fs::CreateDirectories(p);
		}
	}

	Fs::Path Project::CreateNewBuildResultDir(BuildOptions options) {
		Fs::Path currentDir = path;
		Fs::Path buildDir = path / "builds";
		CreateDirIfNotExists(buildDir);
		Fs::Path platformDir = buildDir / PlatformAsString(options.platform);
		CreateDirIfNotExists(platformDir);
		buildDir = platformDir / (name + "_" + CurrentDateStamp());
		CreateDirIfNotExists(buildDir);
		return buildDir;
	}

	Project::BuildResult Project::Build(BuildOptions options) {
		Logger().BeginSection("Building " + name);
		auto timer = SimpleTimer::Start();

		BuildResult result;
		result.status = BuildStatus::Successful;
		result.path = CreateNewBuildResultDir(options);
		result.timeInSeconds = timer.ElapsedSeconds();
		result.sizeInBytes = 0;

		// TODO: actually build something

		if (result.status == BuildStatus::Successful) {
			Logger().Log("Build successful");
			Logger().Log("Size: " + std::to_string(result.sizeInBytes) + " bytes");
		}
		else {
			Logger().Log("Build failed");
		}
		Logger().Log("Time elapsed: " + std::to_string(result.timeInSeconds) + " seconds");
		Logger().EndSection();

		return result;
	}
}