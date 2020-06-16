#include "catch.hpp"
#include "AssetManagement/AssetPacker.h"
#include "AssetManagement/AssetReader.h"
#include "Utils/PlatformMacros.h"
#include "Utils/UuidGenerator.h"
#ifdef SIS_WINDOWS
#include <filesystem>
#endif
#include <fstream>
#include <vector>

using namespace Sisyphus;
using namespace Sisyphus::AssetManagement;

// Asset Packer is Windows only
#ifdef SIS_WINDOWS
TEST_CASE("Asset Packer") {
	std::filesystem::path srcPath = "../test_data/AssetPacker/Assets";
	std::filesystem::path destPath = "../test_data/AssetPacker/Assets_Packed";
	std::filesystem::remove_all(destPath);
	std::filesystem::create_directories(destPath);
	auto reader = AssetReader::Create(AssetReaderType::Unpacked);
	reader->Read(srcPath.string());
	REQUIRE(reader->AssetCount() == 4);
	AssetPacker packer;
	packer.PackAssets(*reader, destPath.string());
}
#endif