#include "RawDataView.h"
#include "RawData.h"
#include "Utils/DebugAssert.h"

namespace Sisyphus {
	RawDataView::RawDataView(RawData& inData) :
		data(&inData)
	{
		data->AddView(this);
	}

	RawDataView::~RawDataView()
	{
		data->ReleaseView(this);
	}

	RawDataView::RawDataView(const RawDataView& other):
		RawDataView(*(other.data))
	{
	}

	RawDataView& RawDataView::operator=(const RawDataView& other)
	{
		data->ReleaseView(this);
		data = other.data;
		data->AddView(this);
		return *this;
	}
	void* RawDataView::Ptr() const
	{
		return data->Ptr();
	}
	size_t RawDataView::Size() const
	{
		return data->Size();
	}
	std::string RawDataView::AsString() const
	{
		SIS_DEBUGASSERT(data->Ptr());
		return std::string(reinterpret_cast<char*>(data->Ptr()), data->Size());
	}
}
