#pragma once
#include "Utils/Filesystem.h"

namespace AssetManagement {
	class AssetManager {
	public:
		AssetManager(Path assetFolder);

	private:
		void InitAssets();

	};
}