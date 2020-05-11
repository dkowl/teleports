#include "AssetPacker.h"
#include <unordered_map>
#include <set>
#include <fstream>
#include "Utils/Json.h"
#include "Utils/Throw.h"
#include "Logger/Logger.h"
#include "Filesystem/Filesystem.h"

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
			Logger().Log("Asset count: " + std::to_string(bundle.second.size()));

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

			auto headerString = header.dump();
			outStream << headerString;
			size_t bundleSize = headerString.size();

			for (auto&& assetId : bundle.second) {
				const auto& asset = source.GetAsset(assetId);
				Logger().Log("Writing asset: " + asset.Metadata().Name());
				outStream.write(reinterpret_cast<const char*>(asset.Data().Ptr()), asset.Data().Size());
				bundleSize += asset.Data().Size();
			}

			Logger().Log("Bundle written, total size: " + std::to_string(bundleSize) + " bytes.");
			Logger().EndSection();
		}
	}

	void AssetPacker::PackAssets(const AssetReader& source, const Fs::Path& destDir) {
		Logger().Log("Asset packing started, initializing bundles...");
		auto bundles = InitBundles(source);

		for(auto&& bundle : bundles) {
			auto bundlePath = destDir / uuids::to_string(bundle.first);
			Logger().BeginSection("Writing bundle, id: " + uuids::to_string(bundle.first) + ", path: " + bundlePath.String());

			if (!Fs::Exists(destDir)) {
				Logger().Log("Directory " + destDir.String() + " does not exist, creating...");
				Fs::CreateDirectories(destDir);
			}

			std::ofstream bundleFile(bundlePath.String(), std::ios::out | std::ios::binary);
			SIS_THROWASSERT_MSG(bundleFile.good(), "Failed to open bundle file!");
			WriteBundle(source, bundle, bundleFile);
		}
	}
}