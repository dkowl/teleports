#pragma once
#include "Asset.h"
#include "AssetMetadata.h"
#include "RawData.h"
#include "Filesystem/Path.h"
#include <mutex>

namespace Sisyphus::AssetManagement {
	class AssetUnpacked : public Asset {
	public:
		AssetUnpacked(const Fs::Path& path);

		RawDataView Data() const override;
		const AssetMetadata& Metadata() const override;

	private:
		void LazyLoadData() const;
		void ReadData() const;

		AssetMetadata metadata;
		Fs::Path path;
		// lazy loaded
		mutable RawData data;
		mutable std::mutex dataMutex;
	};

}