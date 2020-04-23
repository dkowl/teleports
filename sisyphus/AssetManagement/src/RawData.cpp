#include "RawData.h"
#include "Utils/DebugAssert.h"
#include "Utils/Throw.h"
#include "Logger/Logger.h"
#include <mutex>

namespace Sisyphus {
	RawData::RawData():
		address(nullptr),
		size(0)
	{
	}

	RawData::RawData(size_t size):
		RawData()
	{
		Init(size);
	}

	RawData::~RawData() {		
		Release();
	}

	void RawData::Init(size_t inSize)
	{
		SIS_THROWASSERT(Empty() && inSize > 0);
		address = malloc(inSize);
		size = inSize;
	}

	void RawData::Release() noexcept
	{
		if (!Empty()) {
			free(address);
			address = nullptr;
		}
	}

	void* RawData::Ptr() const {
		return address;
	}

	size_t RawData::Size() const {
		return size;
	}

	bool RawData::Empty() const
	{
		return address == nullptr;
	}

	int RawData::RefCount() const
	{
		std::lock_guard guard(viewsMutex);
		return static_cast<int>(views.size());
	}

	void RawData::AddView(RawDataView* view) const
	{
		std::lock_guard guard(viewsMutex);
		SIS_DEBUGASSERT(views.find(view) == views.end());
		views.insert(view);
	}

	void RawData::ReleaseView(RawDataView* view) const
	{
		std::lock_guard guard(viewsMutex);
		SIS_DEBUGASSERT(views.find(view) != views.end());
		views.erase(view);
	}
}
