#include "AssetPacker.h"
#include <unordered_map>
#include <set>
#include <fstream>
#include "Utils/Json.h"

namespace Sisyphus::AssetManagement {

	namespace {
		using Bundle = std::pair<uuids::uuid, std::set<uuids::uuid>>;
		using Bundles = std::unordered_map<uuids::uuid, std::set<uuids::uuid>>;

		Bundles InitBundles(const AssetReader& source) {
			std::unordered_map<uuids::uuid, std::set<uuids::uuid>> bundles;
			auto assetIds = source.GetAllAssetIds();
			for (auto&& assetId : assetIds) {
				const auto& asset = source.GetAsset(assetId);
				bundles[asset.Metadata().BundleId()].insert(assetId);
			}
			return bundles;
		}

		void WriteBundle(const AssetReader& source, const Bundle& bundle, std::ostream& outStream) {
			std::size_t curOffset = 0;
			json header{};
			header["assets"] = json::array();

			for(auto&& assetId : bundle.second) {
				const auto& asset = source.GetAsset(assetId);
				json assetJson;
				assetJson["metadata"] = asset.Metadata();
				assetJson["offset"] = curOffset;
				header["assets"].push_back(assetJson);
				curOffset += asset.Data().Size();
			}

			outStream << header;

			for (auto&& assetId : bundle.second) {
				const auto& asset = source.GetAsset(assetId);
				outStream.write(reinterpret_cast<const char*>(asset.Data().Ptr()), asset.Data().Size());
			}
		}
	}

	void AssetPacker::PackAssets(const AssetReader& source, const Fs::Path& destDir) {
		auto bundles = InitBundles(source);

		for(auto&& bundle : bundles) {
			auto bundlePath = destDir / uuids::to_string(bundle.first);
			std::ofstream bundleFile(bundlePath.String(), std::ios::out | std::ios::binary);
			WriteBundle(source, bundle, bundleFile);
		}
	}
}