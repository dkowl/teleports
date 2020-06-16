#pragma once
#include <string>
#include <memory>
#include <vector>
#include "AssetManagement/Asset.h"

namespace Sisyphus::AssetManagement {

	enum class AssetReaderType {
		Unpacked,
		Packed
	};

	class AssetReader {
	public:
		static std::unique_ptr<AssetReader> Create(AssetReaderType type = AssetReaderType::Packed);

		virtual ~AssetReader() = default;

		virtual void Read(const std::string& directory) = 0;
		virtual const Asset& GetAsset(uuids::uuid id) const = 0;
		virtual int AssetCount() const = 0;
		virtual std::vector<uuids::uuid> GetAllAssetIds() const = 0;
	};

}