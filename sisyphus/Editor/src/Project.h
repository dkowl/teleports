#pragma once
#include <string>
#include <memory>
#include "Filesystem/Path.h"
#include "Utils/Platform.h"

namespace Sisyphus::Editor {

	class Project {
	public:
		Project(const Fs::Path& path);
		~Project();

		std::string Name() const;

		void PackAssets();

		struct BuildOptions {
			Platform platform;
		};

		enum class BuildStatus {
			Failed,
			Successful
		};

		struct BuildResult {
			BuildStatus status;
			Fs::Path path;
			double timeInSeconds;
			uint64_t sizeInBytes;
		};
		BuildResult Build(BuildOptions options);

	private:
		Fs::Path CreateNewBuildResultDir(BuildOptions options);

		std::string name;
		Fs::Path path;
		struct PrivateData;
		std::unique_ptr<PrivateData> impl;
	};
}