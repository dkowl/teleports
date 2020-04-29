#include "AssetBundle.h"
#include "Utils/DebugAssert.h"
#include "Utils/Throw.h"
#include "json.hpp"

namespace Sisyphus::AssetManagement {

	void AssetBundle::ReadAssets(std::string path)
	{
		SIS_THROWASSERT(ResourceExists(path));
		
	}
	void AssetBundle::LazyLoadData() const
	{
		if (data.Empty()) {
			ReadData(data);
		}
		SIS_DEBUGASSERT(!data.Empty());
	}
	RawDataView AssetBundle::GetDataView(size_t offset, size_t length) const
	{
		LazyLoadData();
		return RawDataView(data, offset, length);
	}
}
