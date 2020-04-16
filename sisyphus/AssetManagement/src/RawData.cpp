#include "RawData.h"
#include "Utils/DebugAssert.h"
#include <mutex>

namespace Sisyphus {
	RawData::RawData():
		address(nullptr),
		size(0)
	{
	}

	RawData::RawData(size_t size):
		address(malloc(size)),
		size(size)
	{
		SIS_DEBUGASSERT(size > 0);
	}

	RawData::~RawData() {
		free(address);
	}

	void* RawData::Ptr() const {
		return address;
	}

	size_t RawData::Size() const {
		return size;
	}

	bool RawData::Empty() const
	{
		return address != nullptr;
	}

	int RawData::RefCount() const
	{
		std::lock_guard guard(viewsMutex);
		return static_cast<int>(views.size());
	}

	void RawData::AddView(RawDataView* view)
	{
		std::lock_guard guard(viewsMutex);
		SIS_DEBUGASSERT(views.find(view) == views.end());
		views.insert(view);
	}

	void RawData::ReleaseView(RawDataView* view)
	{
		std::lock_guard guard(viewsMutex);
		SIS_DEBUGASSERT(views.find(view) != views.end());
		views.erase(view);
	}
}
