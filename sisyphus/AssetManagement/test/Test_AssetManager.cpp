#include "catch.hpp"
#include "AssetManagement/AssetManager.h"
#include "Utils\FilesystemUtils.h"
#include <fstream>
using namespace Sisyphus;
using namespace Sisyphus::AssetManagement;

TEST_CASE("Asset Manager") {
	Path dirPath = fs::current_path();
	dirPath /= "temp";
	fs::remove_all(dirPath);
	fs::create_directories(dirPath);

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

	AssetManager manager(dirPath.string());
	REQUIRE(manager.GetAssetCount() == 3);
	fs::remove_all(dirPath);
}