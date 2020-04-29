#pragma once
#include "AssetBundle.h"

namespace Sisyphus::AssetManagement {

	class AssetBundleWindows : public AssetBundle {
	protected:
		bool ResourceExists(const std::string& path) const override;
		std::string ReadHeader(const std::string& path) const override;
	};

}