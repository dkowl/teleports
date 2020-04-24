#include "catch.hpp"
#include "AssetManagement/Asset.h"
#include "AssetManagement/AssetUnpacked.h"
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
	Fs::Path dirPath = fs::current_path().string();
	dirPath /= "temp";
	fs::remove_all(dirPath.String());
	fs::create_directories(dirPath.String());
	Fs::Path assetPath = dirPath / "asset.txt";
	auto createAsset = [assetPath]() {return AssetUnpacked(assetPath); };
	SECTION("No file") {
		REQUIRE_THROWS(createAsset());
	}
	std::ofstream assetFile(assetPath.String());
	String content = "Content\nContent\nContent";
	assetFile << content;
	assetFile.close();

	AssetUnpacked asset(assetPath);
	Path metaPath = assetPath.String();
	metaPath += ".meta";
	REQUIRE(fs::exists(metaPath));
	auto id = asset.Metadata().Id();
	REQUIRE(id == asset.Metadata().BundleId());

	SECTION("GetDataAsString") {
		auto data = asset.Data().AsString();
		REQUIRE(data == content);
		assetFile.open(assetPath.String());
		assetFile.clear();
		assetFile << "Other Content";
		data = asset.Data().AsString();
		REQUIRE(data == content);
		assetFile.close();
	}

	SECTION("GetData") {
		assetFile.open(assetPath.String(), std::ios::trunc | std::ios::binary);
		char c = 32;
		assetFile << c;
		assetFile.close();
		REQUIRE_NOTHROW(asset.Data());
		auto data = asset.Data();
		REQUIRE(static_cast<char>(((char*)(data.Ptr()))[0]) == c);
	}

	AssetUnpacked asset2(assetPath);
	REQUIRE(id == asset2.Metadata().Id());

	std::ofstream file;
	file.open(metaPath, std::ios::trunc);
	REQUIRE((file.good() && file.is_open()));
	file << "Random stuff";
	file.close();

	REQUIRE_THROWS(createAsset());
	fs::remove_all(dirPath.String());
#endif
#ifdef SIS_ANDROID
	std::cout << "No Android tests yet\n";
	REQUIRE(false);
#endif
}
