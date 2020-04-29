#pragma once
#include "Asset.h"

namespace Sisyphus::AssetManagement {
	class AssetBundle;

	class AssetPacked : public Asset {
	public:
		AssetPacked(const AssetBundle& parentBundle, const std::string& metadata, size_t offset, size_t length);

		RawDataView Data() const override;
		const AssetMetadata& Metadata() const override;

	private:
		const AssetBundle* parentBundle;
		AssetMetadata metadata;
		size_t offset;
		size_t length;
	};
}

