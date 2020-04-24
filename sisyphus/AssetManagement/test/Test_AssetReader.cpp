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

TEST_CASE("Asset Reader Unpacked") {
#ifdef SIS_WINDOWS
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

		auto reader = AssetReader::Create(AssetReader::ReaderType::Unpacked);
		reader->ReadAssets(dirPath.string());
		REQUIRE(reader->AssetCount() == 3);
		REQUIRE(reader->GetAllAssetIds().size() == reader->AssetCount());
	}
	catch (...) {
		REQUIRE(false);
	}
	fs::remove_all(dirPath);
#endif
#ifdef SIS_ANDROID
	std::cout << "No Android tests yet\n";
	REQUIRE(false);
#endif
}