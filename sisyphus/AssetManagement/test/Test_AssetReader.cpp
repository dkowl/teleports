#include "catch.hpp"
#include "Utils/PlatformMacros.h"
#include "AssetManagement/AssetReader.h"
#ifdef SIS_WINDOWS
#include "Utils\FilesystemUtils.h"
#endif
#include <fstream>
#include <iostream>
using namespace Sisyphus;
using namespace Sisyphus::AssetManagement;

#ifdef SIS_WINDOWS
TEST_CASE("Asset Reader Unpacked") {
	Path dirPath = fs::current_path();
	dirPath /= "temp";
	fs::remove_all(dirPath);
	fs::create_directories(dirPath);

	try {
		Path file1Path = dirPath / "asset1.asset";
		std::ofstream file1(file1Path);
		file1 << "Content";
		file1.close();

		Path subDirPath = dirPath / "subdir";
		fs::create_directories(subDirPath);
		Path file2Path = subDirPath / "asset2.txt";
		Path file3Path = subDirPath / "asset3.txt";
		std::ofstream file2(file2Path);
		std::ofstream file3(file3Path);
		file2 << "CONTENT2";
		file3 << "Content\nContent\nContent\n";
		file2.close();
		file3.close();

		auto reader = AssetReader::Create(AssetReaderType::Unpacked);
		reader->Read(dirPath.string());
		REQUIRE(reader->AssetCount() == 3);
		REQUIRE(reader->GetAllAssetIds().size() == reader->AssetCount());
	}
	catch (...) {
		REQUIRE(false);
	}
	fs::remove_all(dirPath);
}
#endif

TEST_CASE("AssetReaderPacked") {
	auto reader = AssetReader::Create();
	reader->Read("test_data/AssetPacker/Assets_Packed");
	REQUIRE(reader->AssetCount() == 4);
	std::set<uuids::uuid> bundleIds{
		*uuids::uuid::from_string("5d59b856-2e12-4aad-9499-e880c56d36b7"),
		*uuids::uuid::from_string("353d9ad8-9aa7-4a3d-98a3-6574842ed6a3")
	};
	auto idsFromReader = reader->GetAllAssetIds();
	for (auto&& id : idsFromReader) {
		auto& asset = reader->GetAsset(id);
		REQUIRE(bundleIds.find(asset.Metadata().BundleId()) != bundleIds.end());
	}

	auto& helloWorldAsset = reader->GetAsset(*uuids::uuid::from_string("287e119f-b167-4095-85f7-3821b9825592"));
	REQUIRE(helloWorldAsset.Data().AsString() == "Hello World!");
}