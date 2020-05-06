#pragma once
#include "AssetManagement/RawData.h"
#include <string>
#include <optional>

namespace Sisyphus::AssetManagement {
	class ResourceLoader {
	public:
		ResourceLoader(std::string inPath, bool inIsBinary = true);
		~ResourceLoader();

		std::optional<uint8_t> ReadByte();

		struct LoadResult {
			bool ok;
			size_t size;
		};
		LoadResult Load(RawData& data, size_t offset = 0, size_t length = 0); // length of 0 means 'till the end'
		static LoadResult Load(RawData& data, std::string path, bool isBinary = true);

		void Rewind();

	private:
		struct PrivateData;
		std::unique_ptr<PrivateData> privateData;
	};
}