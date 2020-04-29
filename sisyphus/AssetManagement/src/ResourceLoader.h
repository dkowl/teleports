#pragma once
#include "RawData.h"
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
		LoadResult Load(RawData& data);
		static LoadResult Load(RawData& data, std::string path, bool isBinary = true);

	private:
		struct PrivateData;
		std::unique_ptr<PrivateData> privateData;
	};
}