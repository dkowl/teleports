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
	Path assetPath = dirPath / "asset.txt";
	auto createAsset = [assetPath]() {return Asset(assetPath); };
	SECTION("No file") {
		REQUIRE_THROWS(createAsset());
	}
	std::ofstream assetFile(assetPath);
	String content = "Content\nContent\nContent";
	assetFile << content;
	assetFile.close();

	Asset asset(assetPath);
	Path metaPath = assetPath;
	metaPath += ".meta";
	REQUIRE(fs::exists(metaPath));
	auto id = asset.GetId();

	SECTION("GetDataAsString") {
		auto data = asset.GetDataAsString();
		REQUIRE(data == content);
		assetFile.open(assetPath);
		assetFile.clear();
		assetFile << "Other Content";
		data = asset.GetDataAsString();
		REQUIRE(data == content);
		assetFile.close();
	}

	SECTION("GetData") {
		assetFile.open(assetPath, std::ios::trunc | std::ios::binary);
		char c = 32;
		assetFile << c;
		assetFile.close();
		REQUIRE_NOTHROW(asset.GetData());
		auto data = asset.GetData();
		REQUIRE(static_cast<char>(data[0]) == c);
	}

	Asset asset2(assetPath);
	REQUIRE(id == asset2.GetId());

	std::ofstream file;
	file.open(metaPath, std::ios::trunc);
	REQUIRE((file.good() && file.is_open()));
	file << "Random stuff";
	file.close();

	REQUIRE_THROWS(createAsset());
	fs::remove_all(dirPath);
}
