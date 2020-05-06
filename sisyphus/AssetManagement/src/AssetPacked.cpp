#include "AssetPacked.h"
#include "AssetBundle.h"
#include "Utils/DebugAssert.h"
#include "Utils/Json.h"

namespace Sisyphus::AssetManagement {
	AssetPacked::AssetPacked(const AssetBundle& inParentBundle, const std::string& metadataString, size_t inOffset, size_t inLength):
		parentBundle(&inParentBundle),
		offset(inOffset),
		length(inLength)
	{
		metadata = json::parse(metadataString).get<AssetMetadata>();
	}

	RawDataView AssetPacked::Data() const
	{
		SIS_DEBUGASSERT(parentBundle);
		return parentBundle->GetDataView(offset, length);
	}
	const AssetMetadata& AssetPacked::Metadata() const
	{
		return metadata;
	}
}