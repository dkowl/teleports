#pragma once
// Abstraction over a contigious block of raw memory
// a sweetspot between void*, size_t and std::vector<std::byte>
// Extra feature: it can spawn RawDataViews, which are lightweight objects that register themselves in here upon creation,
// allowing to know a lot about how the resource is used
// might be moved to Utils if something other than AssetManagement uses it
#include <unordered_set>
#include <mutex>

namespace Sisyphus {
	class RawDataView;

	class RawData {
	public:
		RawData();
		RawData(size_t size);
		~RawData();
		// TODO: copy, for now deleted for safety
		RawData(const RawData&) = delete;
		RawData& operator=(const RawData&) = delete;
		// moves deleted because RawDataViews track the address in memory
		RawData(RawData&& other) = delete;
		RawData& operator=(RawData&& other) = delete;

		void* Ptr() const;
		size_t Size() const;
		bool Empty() const;
		int RefCount() const;
	private:
		friend class RawDataView;
		void AddView(RawDataView* view);
		void ReleaseView(RawDataView* view);

		void* address;
		size_t size;
		mutable std::mutex viewsMutex;
		std::unordered_set<RawDataView*> views;
	};
}