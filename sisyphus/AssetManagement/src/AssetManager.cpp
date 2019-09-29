#include "AssetManager.h"

namespace AssetManagement {
	AssetManager::AssetManager(Path inAssetDir):
		mainAssetDir(inAssetDir)
	{
		InitAssets();
	}
	Vector<Path> AssetManager::FindAllAssetPaths(Path dir)
	{
		Vector<Path> result;
		for (auto&& p : fs::recursive_directory_iterator(dir)) {
			if (p.is_regular_file() && p.path().extension() != ".meta") {
				result.push_back(p);
			}
		}
		return result;
	}
	void AssetManager::InitAssets()
	{
		auto paths = FindAllAssetPaths(mainAssetDir);
		for (auto&& p : paths) {
			AddAsset(std::make_unique<Asset>(p));
		}
	}
	void AssetManager::AddAsset(std::unique_ptr<Asset> asset)
	{
		assets.insert({ asset->GetId(), std::move(asset) });
	}
}