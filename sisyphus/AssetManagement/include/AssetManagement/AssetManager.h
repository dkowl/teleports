#pragma once
#include "Utils/FilesystemUtils.h"

namespace AssetManagement {
	class AssetManager {
	public:
		AssetManager(Path assetFolder);

	private:
		void InitAssets();

	};
}