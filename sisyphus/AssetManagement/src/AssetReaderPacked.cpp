#include "AssetReaderPacked.h"
#include "Filesystem/Filesystem.h"
#include "Filesystem/Path.h"
#include "Utils/Throw.h"
#include <vector>
#include "Utils/Platform.h"
#ifdef SIS_ANDROID
#include "AndroidGlobals/Globals.Android.h"
#include "android/asset_manager.h"
#endif

namespace Sisyphus::AssetManagement {
	namespace {
		std::vector<Fs::Path> FindAllBundlePaths(const std::string& dir)
		{
			std::vector<Fs::Path> result;
#ifdef SIS_WINDOWS
			for (auto&& p : Fs::RecursiveDirectoryIterator(dir)) {
				if (Fs::IsRegularFile(p) && p.Extension().String() != ".meta") {
					result.push_back(p);
				}
			}
#elif defined(SIS_ANDROID)
			auto assetDir = AAssetManager_openDir(AndroidGlobals::AssetManager(), dir.c_str());
			while (true) {
				auto filename = AAssetDir_getNextFileName(assetDir);
				if (filename == nullptr) break;
				result.push_back(Fs::Path(dir) / std::string(filename));
			}
#endif
			return result;
		}
	}

	void AssetReaderPacked::Read(const std::string& dir)
	{
		auto paths = FindAllBundlePaths(dir);

		for (auto&& path : paths) {			
			AddBundle(std::make_unique<AssetBundle>(path.String()));			
		}
	}

	const Asset& AssetReaderPacked::GetAsset(uuids::uuid id) const
	{
		// this gives errors because it's a const method
		auto bundleFindResult = assetToBundleIdMap.find(id);
		SIS_THROWASSERT(bundleFindResult != assetToBundleIdMap.end());
		uuids::uuid bundleId = bundleFindResult->second;
		
		auto& bundle = bundles.at(bundleId);
		return bundle->GetAsset(id);
	}

	int AssetReaderPacked::AssetCount() const
	{
		int count = 0;
		for (auto&& bundle : bundles) {
			count += bundle.second->AssetCount();
		}
		return count;
	}

	std::vector<uuids::uuid> AssetReaderPacked::GetAllAssetIds() const
	{
		std::vector<uuids::uuid> result;
		for (auto&& bundle : bundles) {
			auto assets = bundle.second->GetAllAssetIds();
			result.insert(result.end(), assets.begin(), assets.end());
		}
		return result;
	}

	void AssetReaderPacked::AddBundle(std::unique_ptr<AssetBundle> bundle)
	{
		auto assetIds = bundle->GetAllAssetIds();
		for (auto&& id : assetIds) {
			assetToBundleIdMap[id] = bundle->Id();
		}
		bundles[bundle->Id()] = std::move(bundle);
	}
}
