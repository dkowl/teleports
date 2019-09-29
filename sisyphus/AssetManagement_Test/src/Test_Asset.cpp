#include "catch.hpp"
#include "AssetManagement/Asset.h"
#include "Utils\FilesystemUtils.h"
#include <fstream>
using namespace AssetManagement;

TEST_CASE("Asset") {
	Path dirPath = fs::current_path();
	dirPath /= "temp";
	fs::remove_all(dirPath);
	fs::create_directories(dirPath);
	Path assetPath = dirPath / "asset.asset";
	Asset asset(assetPath);
	Path metaPath = assetPath;
	metaPath += ".meta";
	REQUIRE(fs::exists(metaPath));
	auto id = asset.GetId();

	Asset asset2(assetPath);
	REQUIRE(id == asset2.GetId());

	std::ofstream file;
	file.open(metaPath, std::ios::trunc);
	REQUIRE((file.good() && file.is_open()));
	file << "Random stuff";

	auto lambda = [assetPath]() {return Asset(assetPath); };
	REQUIRE_THROWS(lambda());
}
