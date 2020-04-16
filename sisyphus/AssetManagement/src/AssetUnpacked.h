#pragma once
#include "Asset.h"
#include "AssetMetadata.h"
#include "RawData.h"
#include "Filesystem/Path.h"

namespace Sisyphus::AssetManagement {
	class AssetUnpacked : public Asset {
	public:
		AssetUnpacked(const Fs::Path& path);

		//const RawData& Data() const override;
		//const AssetMetadata& Metadata() const override;

	private:
		AssetMetadata metadata;
		RawData data;
	};

}