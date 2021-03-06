#include "Pch_AssetManagement.h"
#include "AssetManager.h"
#include "Utils\Logger.h"

namespace Sisyphus::AssetManagement {
	AssetManager::AssetManager(Path inAssetDir):
		mainAssetDir(inAssetDir)
	{
		InitAssets();
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
		Logger::Get().BeginSection("Asset detected");
		Logger::Get().Log("Id: " + uuids::to_string(asset->GetId()));
		Logger::Get().Log("Name: " + asset->GetName());
		Logger::Get().EndSection();
		assets.insert({ asset->GetId(), std::move(asset) });
	}
	const Asset& AssetManager::GetAsset(uuids::uuid id) const
	{
		auto findResult = assets.find(id);
		if (findResult == assets.end()) {
			throw std::runtime_error("Cannot find asset with id " + uuids::to_string(id));
		}
		return *(*findResult).second;
	}
	int AssetManager::GetAssetCount() const
	{
		return static_cast<int>(assets.size());
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
}