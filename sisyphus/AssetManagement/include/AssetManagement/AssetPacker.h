#pragma once
#include "AssetReader.h"
#include "Filesystem/Path.h"

namespace Sisyphus::AssetManagement {

	// an AssetPacker takes an AssetReader and transforms its content into a packed form on the disk
	// in the packed form there is a flat directory structure and one file per bundle, named after its bundle's id
	class AssetPacker {
	public:
		void PackAssets(const AssetReader& source, const Fs::Path& destDir);
	};
}