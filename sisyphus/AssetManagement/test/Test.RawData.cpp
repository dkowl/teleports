#include "catch.hpp"

#include "AssetManagement/RawData.h"
#include "AssetManagement/RawDataView.h"

using namespace Sisyphus;

TEST_CASE("RawData - RefCount") {

	// Reference counting
	RawData data(10000);
	REQUIRE(data.RefCount() == 0);
	{
		RawDataView view1(data);
		REQUIRE(data.RefCount() == 1);
		{
			RawDataView view2 = view1;
			REQUIRE(data.RefCount() == 2);
		}
		REQUIRE(data.RefCount() == 1);
		RawDataView view3(view1);
		REQUIRE(data.RefCount() == 2);
	}
	REQUIRE(data.RefCount() == 0);
}

TEST_CASE("RawData - Data Access") {
	std::string str = "Hello World!";
	RawData data(str.size());
	memcpy(data.Ptr(), str.c_str(), data.Size());
	RawDataView view(data);
	REQUIRE(view.AsString() == str);
}