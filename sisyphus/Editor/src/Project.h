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

		enum class Configuration {
			Debug,
			Release
		};
		static std::string ConfigurationAsString(Configuration config);

		Fs::Path ProjectDir(Platform platform) const;

		void PackAssets();

		struct ReleaseOptions {
			Platform platform;
		};

		enum class ReleaseStatus {
			Failed,
			Successful
		};

		struct ReleaseResult {
			ReleaseStatus status;
			Fs::Path path;
			double timeInSeconds;
			uint64_t sizeInBytes;
		};
		ReleaseResult Release(ReleaseOptions options);

	private:
		Fs::Path CreateNewReleaseResultDir(ReleaseOptions options);

		std::string name;
		Fs::Path path;
		struct PrivateData;
		std::unique_ptr<PrivateData> impl;
	};
}