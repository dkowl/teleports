#include "AssetReaderPacked.Windows.h"
#include "Filesystem/Filesystem.h"
#include "Filesystem/Path.h"
#include "Utils/Throw.h"
#include <vector>

namespace Sisyphus::AssetManagement {
	namespace {
		std::vector<Fs::Path> FindAllBundlePaths(const std::string& dir)
		{
			std::vector<Fs::Path> result;
			for (auto&& p : Fs::RecursiveDirectoryIterator(dir)) {
				if (Fs::IsRegularFile(p) && p.Extension().String() != ".meta") {
					result.push_back(p);
				}
			}
			return result;
		}
	}

	void AssetReaderPacked::ReadAssets(std::string dir)
	{
		auto paths = FindAllBundlePaths(dir);

		for (auto&& path : paths) {
			auto id = uuids::uuid::from_string(path.Filename().String());
			SIS_THROWASSERT_MSG(id, "Invalid asset bundle file name: " + path.Filename().String());
			// bundles[id.value()] = AssetBundle(path
		}
	}
}
