#pragma once
#include <string>
#include <memory>
#include <vector>
#include "AssetManagement/Asset.h"

namespace Sisyphus::AssetManagement {
	class AssetReader {
	public:
		enum class ReaderType {
			Unpacked,
			Packed
		};

		static std::unique_ptr<AssetReader> Create(ReaderType type = ReaderType::Packed);

		virtual ~AssetReader() = default;

		virtual void ReadAssets(const std::string& directory) = 0;
		virtual const Asset& GetAsset(uuids::uuid id) const = 0;
		virtual int AssetCount() const = 0;
		virtual std::vector<uuids::uuid> GetAllAssetIds() const = 0;
	};

}