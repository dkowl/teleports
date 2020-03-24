#include "catch.hpp"
#include "AssetManagement/Asset.h"
#include "Utils/PlatformMacros.h"
#ifdef SIS_WINDOWS
#include "Utils\FilesystemUtils.h"
#endif
#include <fstream>
#include <iostream>
using namespace Sisyphus;
using namespace Sisyphus::AssetManagement;

TEST_CASE("Asset") {
#ifdef SIS_WINDOWS
	Path dirPath = fs::current_path();
	dirPath /= "temp";
	fs::remove_all(dirPath);
	fs::create_directories(dirPath);
	Path assetPath = dirPath / "asset.txt";
	auto createAsset = [assetPath]() {return Asset(assetPath.string()); };
	SECTION("No file") {
		REQUIRE_THROWS(createAsset());
	}
	std::ofstream assetFile(assetPath);
	String content = "Content\nContent\nContent";
	assetFile << content;
	assetFile.close();

	Asset asset(assetPath.string(), false);
	Path metaPath = assetPath;
	metaPath += ".meta";
	REQUIRE(fs::exists(metaPath));
	auto id = asset.Id();

	SECTION("GetDataAsString") {
		auto data = asset.DataAsString();
		REQUIRE(data == content);
		assetFile.open(assetPath);
		assetFile.clear();
		assetFile << "Other Content";
		data = asset.DataAsString();
		REQUIRE(data == content);
		assetFile.close();
	}

	SECTION("GetData") {
		assetFile.open(assetPath, std::ios::trunc | std::ios::binary);
		char c = 32;
		assetFile << c;
		assetFile.close();
		REQUIRE_NOTHROW(asset.Data());
		auto data = asset.Data();
		REQUIRE(static_cast<char>(data[0]) == c);
	}

	Asset asset2(assetPath.string());
	REQUIRE(id == asset2.Id());

	std::ofstream file;
	file.open(metaPath, std::ios::trunc);
	REQUIRE((file.good() && file.is_open()));
	file << "Random stuff";
	file.close();

	REQUIRE_THROWS(createAsset());
	fs::remove_all(dirPath);
#endif
#ifdef SIS_ANDROID
	std::cout << "No Android tests yet\n";
	REQUIRE(false);
#endif
}
