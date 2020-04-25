#pragma once
#include <string_view>

namespace Sisyphus {
	class RawData;

	class RawDataView {
	public:
		RawDataView();
		RawDataView(const RawData& inData, size_t inOffset = 0, size_t inLength = 0); // length of 0 means 'till the end'
		~RawDataView();
		RawDataView(const RawDataView& other);
		RawDataView& operator=(const RawDataView& other);
		// the views are tracked by address in RawData and we don't want a view's address to change randomly, 
		// so moves are disallowed to keep things simple
		RawDataView(RawDataView&& other) = delete;
		RawDataView& operator=(RawDataView&& other) = delete;

		void* Ptr() const;
		size_t Size() const;
		// string_view was previously considered, but that would create an untracked reference to the resource
		std::string AsString() const;
		bool Empty() const;
	private:
		void Init(const RawData* inData, size_t inOffset = 0, size_t inLength = 0);
		void Release();

		const RawData* data;
		size_t offset;
		size_t length;
	};
}