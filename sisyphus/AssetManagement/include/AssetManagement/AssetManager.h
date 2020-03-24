#pragma once
#include <map>
#include <vector>
#include <string>
#include "Asset.h"
#include <memory>

namespace Sisyphus::AssetManagement {
	class AssetManager {
	public:
		AssetManager(std::string inAssetDir, bool isReadOnly = true);

		const Asset& GetAsset(uuids::uuid id) const;
		int GetAssetCount() const;

	private:
		static std::vector<std::string> FindAllAssetPaths(std::string dir);
		void InitAssets(bool isReadOnly);
		void AddAsset(std::unique_ptr<Asset> asset);

		std::string mainAssetDir;
		std::map<uuids::uuid, std::unique_ptr<Asset>> assets;
	};
}