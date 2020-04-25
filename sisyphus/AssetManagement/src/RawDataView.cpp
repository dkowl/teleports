#include "RawDataView.h"
#include "RawData.h"
#include "Utils/DebugAssert.h"
#include "Utils/Throw.h"
#include "Utils/Logger.h"

namespace Sisyphus {
	RawDataView::RawDataView():
		data(nullptr),
		offset(0),
		length(0)
	{
	}

	RawDataView::RawDataView(const RawData& inData, size_t inOffset, size_t inLength):
		RawDataView()
	{
		Init(&inData, inOffset, inLength);
	}

	RawDataView::~RawDataView()
	{
		try {
			Release();
		}
		catch (...) {
			Logger().Log("Exception thrown when releasing a RawDataView!");
		}
	}

	RawDataView::RawDataView(const RawDataView& other):
		RawDataView(*(other.data), other.offset, other.length)
	{
	}

	RawDataView& RawDataView::operator=(const RawDataView& other)
	{
		Release();
		Init(other.data, other.offset, other.length);
		return *this;
	}
	void* RawDataView::Ptr() const
	{
		return reinterpret_cast<char*>(data->Ptr()) + offset;
	}
	size_t RawDataView::Size() const
	{
		return length;
	}
	std::string RawDataView::AsString() const
	{
		SIS_DEBUGASSERT(data->Ptr());
		return std::string(reinterpret_cast<char*>(data->Ptr()) + offset, length);
	}
	bool RawDataView::Empty() const
	{
		return data == nullptr;
	}
	void RawDataView::Init(const RawData* inData, size_t inOffset, size_t inLength)
	{
		SIS_THROWASSERT(Empty());
		data = inData;
		offset = inOffset;
		if (data) {
			SIS_DEBUGASSERT(inOffset + inLength <= data->Size());
			length = inLength == 0 ? data->Size() - inOffset : inLength;
			data->AddView(this);
		}
	}
	void RawDataView::Release()
	{
		if (data) {
			data->ReleaseView(this);
			data = nullptr;
			offset = 0;
			length = 0;
		}
	}
}
