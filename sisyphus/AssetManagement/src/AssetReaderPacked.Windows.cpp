#include "AssetReaderPacked.Windows.h"
#include "Filesystem/Filesystem.h"
#include "Filesystem/Path.h"
#include <vector>

namespace Sisyphus::AssetManagement {
	namespace {
		std::vector<std::string> FindAllAssetPaths(const std::string& dir)
		{
			std::vector<std::string> result;
			for (auto&& p : Fs::RecursiveDirectoryIterator(dir)) {
				if (Fs::IsRegularFile(p) && p.Extension().String() != ".meta") {
					result.push_back(p.String());
				}
			}
			return result;
		}
	}

	void AssetReaderPacked::ReadAssets(std::string dir)
	{
		auto paths = FindAllAssetPaths(dir);

	}
}
