#include "catch.hpp"

#include "AssetManagement/RawData.h"
#include "AssetManagement/RawDataView.h"

#include <thread>
#include <iostream>

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

TEST_CASE("Thread safety") {
	RawData data(10000);

	auto create = [&data](int n, std::string tag) {
		for (int i = 0; i < n; i++) {
			// std::cout << tag << ": all destroyed\n";
			RawDataView view1(data);
			// std::cout << tag << ": created 1\n";
			RawDataView view2(view1);
			// std::cout << tag << ": created 2\n";
			RawDataView view3 = view2;
			// std::cout << tag << ": created 3\n";
		}
	};

	auto read = [&data](int n) {
		for (int i = 0; i < n; i++) {
			// std::cout << "reading: " << data.RefCount() << "\n";
			REQUIRE(data.RefCount() <= 6);
		}
	};

	auto execute = [&](int n) {
		std::thread creator1(create, n, "creator1");
		std::thread reader(read, n * 10);
		std::thread creator2(create, n, "creator2");

		creator1.join();
		reader.join();
		creator2.join();
	};

	try {
		REQUIRE_NOTHROW(execute(1000));
	}
	catch (...) {

	}
}