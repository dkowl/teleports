#pragma once
#include "AssetReader.h"
#include "AssetBundle.h"
#include <unordered_map>
#include <memory>
#include "uuid.h"

namespace Sisyphus::AssetManagement {
	class AssetReaderPacked : public AssetReader {
	public:
		void ReadAssets(const std::string& dir) override;
		const Asset& GetAsset(uuids::uuid id) const override;
		int AssetCount() const override;
		std::vector<uuids::uuid> GetAllAssetIds() const override;

	private:
		void AddBundle(std::unique_ptr<AssetBundle> bundle);

		std::unordered_map<uuids::uuid, std::unique_ptr<AssetBundle>> bundles;
		std::unordered_map<uuids::uuid, uuids::uuid> assetToBundleIdMap;
	};
}