#include "AssetReaderUnpacked.Windows.h"
#include "AssetUnpacked.h"
#include "Filesystem/Filesystem.h"
#include "Utils/Logger.h"

namespace Sisyphus::AssetManagement {
	namespace {
		std::vector<std::string> FindAllAssetPaths(std::string dir)
		{
			std::vector<std::string> result;
			for (auto&& p : Fs::RecursiveDirectoryIterator(dir)) {
				if (Fs::IsRegularFile(p) && p.Extension().String() != ".meta") {
					result.push_back(p.String());
				}
			}
			return result;
		}
	}

	void AssetReaderUnpacked::ReadAssets(std::string directory) {
		auto paths = FindAllAssetPaths(directory);
		for (auto&& p : paths) {
			AddAsset(std::make_unique<AssetUnpacked>(p));
		}
	}

	const Asset& AssetReaderUnpacked::GetAsset(uuids::uuid id) const
	{
		auto findResult = assets.find(id);
		if (findResult == assets.end()) {
			throw std::runtime_error("Cannot find asset with id " + uuids::to_string(id));
		}
		return *(*findResult).second;
	}

	int AssetReaderUnpacked::AssetCount() const
	{
		return static_cast<int>(assets.size());
	}

	void AssetReaderUnpacked::AddAsset(std::unique_ptr<Asset> asset)
	{
		Logger().BeginSection("Asset detected");
		Logger().Log("Id: " + uuids::to_string(asset->Metadata().Id()));
		Logger().Log("Name: " + asset->Metadata().Name());
		Logger().EndSection();
		assets.insert({ asset->Metadata().Id(), std::move(asset) });
	}
}