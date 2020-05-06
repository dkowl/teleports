#pragma once
#include "AssetReader.h"
#include "RawData.h"
#include <unordered_map>
#include <uuid.h>
#include <memory>

namespace Sisyphus::AssetManagement {

	class AssetPacked;
	class AssetBundle : public AssetReader {
	public:
		AssetBundle();
		AssetBundle(const std::string& inPath);

		~AssetBundle();

		void ReadAssets(const std::string& inPath) override;
		const Asset& GetAsset(uuids::uuid assetId) const override;
		int AssetCount() const override;
		std::vector<uuids::uuid> GetAllAssetIds() const override;

		uuids::uuid Id() const;

	private:
		friend class AssetPacked;
		std::string ReadHeader();
		void AddAsset(const std::string metadata, size_t offset, size_t length);
		RawDataView GetDataView(size_t offset, size_t length) const;
		void LazyLoadData() const;

		mutable std::mutex dataMutex;
		mutable RawData data;
		std::string path;
		uuids::uuid id;
		size_t headerLength;
		std::unordered_map<uuids::uuid, std::unique_ptr<AssetPacked>> assets;
	};
}