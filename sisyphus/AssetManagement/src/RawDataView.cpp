#include "RawDataView.h"
#include "RawData.h"
#include "Utils/DebugAssert.h"
#include "Utils/Throw.h"
#include "Utils/Logger.h"

namespace Sisyphus {
	RawDataView::RawDataView():
		data(nullptr)
	{
	}

	RawDataView::RawDataView(const RawData& inData):
		RawDataView()
	{
		Init(&inData);
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
		RawDataView(*(other.data))
	{
	}

	RawDataView& RawDataView::operator=(const RawDataView& other)
	{
		Release();
		Init(other.data);
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
	bool RawDataView::Empty() const
	{
		return data == nullptr;
	}
	void RawDataView::Init(const RawData* inData)
	{
		SIS_THROWASSERT(Empty());
		data = inData;
		if (data) {
			data->AddView(this);
		}
	}
	void RawDataView::Release()
	{
		if (data) {
			data->ReleaseView(this);
			data = nullptr;
		}
	}
}
