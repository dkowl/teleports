#pragma once
#include <string>
#include "AssetManagement/Asset.h"

class AssetIterator;
namespace Sisyphus::AssetManagement {
	class AssetReader {
	public:
		virtual void ReadAssets(std::string directory) = 0;
		virtual const Asset& GetAsset(uuids::uuid id) const = 0;
		virtual AssetIterator GetIterator() = 0;
	};

}