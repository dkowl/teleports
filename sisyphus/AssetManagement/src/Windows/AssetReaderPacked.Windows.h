#pragma once
#include "AssetReader.h"
#include "AssetBundle.h"
#include <unordered_map>
#include <memory>
#include "uuid.h"

namespace Sisyphus::AssetManagement {
	class AssetReaderPacked : public AssetReader {
	public:
		void ReadAssets(std::string dir);

	private:
		std::unordered_map<uuids::uuid, AssetBundle> bundles;
	};
}