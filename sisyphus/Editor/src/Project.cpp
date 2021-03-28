#include "Project.h"
#include "Utils/DebugAssert.h"
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

	std::string Project::ConfigurationAsString(Configuration config)
	{
		switch (config) {
		case Configuration::Debug:
			return "Debug";
		case Configuration::Release:
			return "Release";
		default:
			SIS_DEBUGASSERT_MSG(false, "Unknown project configuration");
			return "Unknown";
		}
	}

	Fs::Path Project::ProjectDir(Platform platform) const
	{
		return path / PlatformAsString(platform);
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

	Fs::Path Project::CreateNewReleaseResultDir(ReleaseOptions options) {
		Fs::Path currentDir = path;
		Fs::Path buildDir = path / "releases";
		CreateDirIfNotExists(buildDir);
		Fs::Path platformDir = buildDir / PlatformAsString(options.platform);
		CreateDirIfNotExists(platformDir);
		buildDir = platformDir / (name + "_" + CurrentDateStamp());
		CreateDirIfNotExists(buildDir);
		return buildDir;
	}

	struct CopyOperationDescriptor {
		Fs::Path from;
		Fs::Path to;
		bool isDirectory;
	};

	Project::ReleaseResult Project::Release(ReleaseOptions options) {
		Logger().BeginSection("Releasing " + name);
		Logger().BeginSection("Options:");
		Logger().Log(std::string("Platform: ") + PlatformAsString(options.platform));
		Logger().EndSection();
		auto timer = SimpleTimer::Start();

		ReleaseResult result;
		result.status = ReleaseStatus::Successful;
		result.path = CreateNewReleaseResultDir(options);
		result.sizeInBytes = 0;

		std::vector<CopyOperationDescriptor> copiesToDo;
		if (options.platform == Platform::Windows)
		{
			copiesToDo.emplace_back(CopyOperationDescriptor{ 
				path / PlatformAsString(options.platform) / "out" / "x64" / "Release" / (name + ".Windows.exe"), 
				result.path / (name + ".exe"), 
				false 
			});

			copiesToDo.emplace_back(CopyOperationDescriptor{ 
				path / "assets_packed",
				result.path / "assets", 
				true 
			});
		}
		else {
			Logger().Log("Unsupported platform");
			result.status = ReleaseStatus::Failed;
		}

		for (auto&& copyDesc : copiesToDo) {
			if (!Fs::Exists(copyDesc.from)) {
				Logger().Log("File or directory not found: " + copyDesc.from.String());
				result.status = ReleaseStatus::Failed;
			}
			else {
				result.sizeInBytes += Fs::FileSize(copyDesc.from);
			}
		}

		if (result.status != ReleaseStatus::Failed) {
			for (auto&& copyDesc : copiesToDo) {
				// https://stackoverflow.com/questions/33752732/xcopy-still-asking-f-file-d-directory-confirmation#:~:text=7%20Answers&text=The%20%2FI%20switch%20(not%20%2F,such%20prompt%20does%20never%20appear.
				std::string command = std::string(copyDesc.isDirectory ? "x" : "") + "copy \"" + copyDesc.from.String() + "\" \"" + copyDesc.to.String() + "\"" + std::string(copyDesc.isDirectory ? " /i" : "");
				Logger().Log("Executing: " + command);
				system(command.c_str());
			}
		}

		result.timeInSeconds = timer.ElapsedSeconds();

		if (result.status == ReleaseStatus::Successful) {
			Logger().Log("Release successful");
			Logger().Log("Size: " + std::to_string(result.sizeInBytes) + " bytes");
		}
		else {
			Logger().Log("Release failed");
		}
		Logger().Log("Time elapsed: " + std::to_string(result.timeInSeconds) + " seconds");
		Logger().EndSection();

		return result;
	}
}