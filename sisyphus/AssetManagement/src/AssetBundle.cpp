#include "AssetBundle.h"
#include "AssetPacked.h"
#include "Utils/DebugAssert.h"
#include "Utils/Throw.h"
#include "Utils/Json.h"
#include "ResourceLoader.h"
#include "Filesystem/Path.h"

namespace Sisyphus::AssetManagement {

	AssetBundle::AssetBundle():
		headerLength(0)
	{
	}

	AssetBundle::AssetBundle(const std::string& inPath):
		AssetBundle()
	{		
		Read(inPath);
	}

	AssetBundle::~AssetBundle() = default;

	void AssetBundle::Read(const std::string& inPath)
	{
		path = inPath;
		auto fsPath = Fs::Path(path);
		auto idOpt = uuids::uuid::from_string(fsPath.Filename().String());
		SIS_THROWASSERT_MSG(idOpt, "Invalid asset bundle file name: " + fsPath.Filename().String());
		id = *idOpt;
		auto headerJson = json::parse(ReadHeader());
		SIS_DEBUGASSERT(headerJson.contains("assets"));
		json assetsJson = headerJson["assets"];
		SIS_DEBUGASSERT(assetsJson.is_array());
		SIS_DEBUGASSERT(!assetsJson.empty());
		std::string metadata;
		size_t offset = 0;
		size_t nextOffset;
		size_t length;
		for (auto&& assetData : assetsJson) {
			SIS_DEBUGASSERT(assetData.contains("metadata"));
			SIS_DEBUGASSERT(assetData.contains("offset"));
			if (!metadata.empty()) {
				nextOffset = assetData["offset"].get<size_t>();
				length = nextOffset - offset;
				AddAsset(metadata, offset, length);
				offset = nextOffset;
			}
			metadata = assetData["metadata"].dump();
		}
		AddAsset(metadata, offset, 0);
	}
	const Asset& AssetBundle::GetAsset(uuids::uuid assetId) const
	{
		auto findResult = assets.find(assetId);
		SIS_THROWASSERT(findResult != assets.end());
		return *(findResult->second);
	}
	int AssetBundle::AssetCount() const
	{
		return static_cast<int>(assets.size());
	}
	std::vector<uuids::uuid> AssetBundle::GetAllAssetIds() const
	{
		std::vector<uuids::uuid> result;
		for (auto&& asset : assets) {
			result.push_back(asset.first);
		}
		return result;
	}
	uuids::uuid AssetBundle::Id() const
	{
		return id;
	}
	void AssetBundle::LazyLoadData() const
	{
		if (data.Empty()) {
			ResourceLoader loader(path);
			loader.Load(data, headerLength);
		}
		SIS_DEBUGASSERT(!data.Empty());
	}
	std::string AssetBundle::ReadHeader()
	{
		ResourceLoader loader(path);
		SIS_THROWASSERT_MSG(static_cast<char>(*loader.ReadByte()) == '{', "Invalid AssetBundle header");
		int openCount = 1;
		int closeCount = 0;
		headerLength = 1;
		while (openCount > closeCount) {
			char c = static_cast<char>(*loader.ReadByte());
			if (c == '{') openCount++;
			else if (c == '}') closeCount++;
			headerLength++;
		}
		loader.Rewind();
		RawData headerData;
		loader.Load(headerData, 0, headerLength);
		return RawDataView(headerData).AsString();
	}
	void AssetBundle::AddAsset(const std::string metadata, size_t offset, size_t length)
	{
		auto asset = std::make_unique<AssetPacked>(*this, metadata, offset, length);
		assets[asset->Metadata().Id()] = std::move(asset);
	}
	RawDataView AssetBundle::GetDataView(size_t offset, size_t length) const
	{
		LazyLoadData();
		return RawDataView(data, offset, length);
	}
}
