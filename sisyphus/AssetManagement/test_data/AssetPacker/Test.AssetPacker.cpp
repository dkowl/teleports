#include "catch.hpp"
#include "AssetManagement/AssetPacker.h"
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
	std::filesystem::path path = "temp/Assets";
	std::vector<uuids::uuid> assetUuids = { GenerateUuid(), GenerateUuid(), GenerateUuid() };
	uuids::uuid bundleId_1 = GenerateUuid();
	uuids::uuid bundleId_2 = GenerateUuid();


}
#endif