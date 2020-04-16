#pragma once
#include <string_view>

namespace Sisyphus {
	class RawData;

	class RawDataView {
	public:
		RawDataView(RawData& inData);
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
	private:
		RawData* data;
	};
}