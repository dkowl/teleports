#include "AssetUnpacked.h"
#include "Filesystem/Filesystem.h"
#include "Utils/Logger.h"
#include "Utils/Throw.h"
#include "ResourceLoader.h"

namespace Sisyphus::AssetManagement {

	AssetUnpacked::AssetUnpacked(const Fs::Path& inPath):
		metadata(inPath.String(), false),
		path(inPath)
	{
		SIS_THROWASSERT(Fs::Exists(inPath));
	}
	RawDataView AssetUnpacked::Data() const
	{
		std::lock_guard guard(dataMutex);
		LazyLoadData();
		return RawDataView(data);
	}
	const AssetMetadata& AssetUnpacked::Metadata() const
	{
		return metadata;
	}
	void AssetUnpacked::LazyLoadData() const
	{
		if (data.Empty()) {
			ReadData();
		}
	}
	void AssetUnpacked::ReadData() const
	{
		auto loadResult = ResourceLoader::Load(data, path.String(), metadata.IsBinary());
		SIS_THROWASSERT(loadResult.ok);

		Logger().BeginSection("Asset file read from disk");
		Logger().Log("Id: " + uuids::to_string(metadata.Id()));
		Logger().Log("Name: " + metadata.Name());
		Logger().Log("Size: " + std::to_string(loadResult.size) + " bytes");
		Logger().EndSection();
	}
}