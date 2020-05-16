#include "catch.hpp"
#include "AssetManagement/ResourceLoader.h"
#include "AssetManagement/RawData.h"
#include "AssetManagement/RawDataView.h"
#include <string>

using namespace Sisyphus;
using namespace Sisyphus::AssetManagement;

std::string ReadBytesToString(ResourceLoader& loader) {
	std::string s;
	while (true) {
		auto byte = loader.ReadByte();
		if (!byte) break;
		s += static_cast<char>(*byte);
	}
	return s;
}

TEST_CASE("Resource Loader") {
	ResourceLoader loader("test_data/ResourceLoader/resource.txt");

	RawData data;
	loader.Load(data);
	RawDataView dataView(data);
	REQUIRE(dataView.AsString() == "Hello World!");
	
	REQUIRE(ReadBytesToString(loader) == "Hello World!");
	REQUIRE(ReadBytesToString(loader) == "");
	loader.Rewind();
	REQUIRE(ReadBytesToString(loader) == "Hello World!");

	RawData data2;
	loader.Load(data2, 6);
	RawDataView dataView2(data2);
	REQUIRE(dataView2.AsString() == "World!");

	RawData data3;
	loader.Load(data3, 6, 5);
	RawDataView dataView3(data3);
	REQUIRE(dataView3.AsString() == "World");

	ResourceLoader emptyLoader("test_data/ResourceLoader/empty.txt");
	RawData emptyData;
	auto loadResult = emptyLoader.Load(emptyData);
	REQUIRE(!loadResult.ok);

	auto makeDirLoader = []() {ResourceLoader dirLoader("test_data/ResourceLoader"); };
	REQUIRE_THROWS(makeDirLoader());	
}