#pragma once
#include "AssetReader.h"
#include "RawData.h"

namespace Sisyphus::AssetManagement {

	class AssetBundle : public AssetReader {
	public:
		AssetBundle();

		void ReadAssets(std::string path) override;
		const Asset& GetAsset(uuids::uuid id) const override;
		int AssetCount() const override;
		std::vector<uuids::uuid> GetAllAssetIds() const override;

	protected:
		virtual bool ResourceExists(const std::string& path) const = 0;
		virtual std::string ReadHeader(const std::string& path) const = 0;
		virtual void ReadData(RawData& outData) const = 0;

	private:
		friend class AssetPacked;
		RawDataView GetDataView(size_t offset, size_t length) const;
		void LazyLoadData() const;

		mutable std::mutex dataMutex;
		mutable RawData data;
	};
}