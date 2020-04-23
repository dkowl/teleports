#pragma once
#include "AssetReader.h"

namespace Sisyphus::AssetManagement {
	class AssetReaderUnpacked : public AssetReader {
	public:
		void ReadAssets(std::string directory) override;
		const Asset& GetAsset(uuids::uuid id) const override;
		int AssetCount() const override;
	private:
		void AddAsset(std::unique_ptr<Asset> asset);
		std::map<uuids::uuid, std::unique_ptr<Asset>> assets;
	};
}
