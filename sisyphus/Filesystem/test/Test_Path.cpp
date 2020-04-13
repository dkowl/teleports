#include "catch.hpp"
#include "Path.h"
#include "Utils/PlatformMacros.h"

using namespace Sisyphus::Fs;

TEST_CASE("Path") {
	Path path("Dir/Dir2/file.cpp");

	REQUIRE(path.Stem().String() == "file");
	REQUIRE(path.Filename().String() == "file.cpp");
	REQUIRE(path.Dirname().String() == "Dir/Dir2/");
	REQUIRE(path.Extension().String() == ".cpp");

	path = "file.h"; // testing operator=
	REQUIRE(path.Extension().String() != ".cpp");
	REQUIRE(path.Extension().String() == ".h");

	Path emptyPath("");
	REQUIRE(emptyPath.Filename().String().empty());

	Path defaultPath;
	REQUIRE(defaultPath.Filename().String().empty());	
}

TEST_CASE("Path - operators") {
	Path path;
	SECTION("operator/=") {
		path = "Dir";
		path /= Path("file.cpp");
	}
	SECTION("template operator/=") {
		path = "Dir";
		path /= "file.cpp";
	}
	SECTION("P / P") {
		path = Path("Dir") / Path("file.cpp");
	}
	SECTION("P / non-P") {
		path = Path("Dir") / std::string("file.cpp");
	}
	SECTION("non-P / P") {
		path = "Dir" / Path("file.cpp");
	}

	REQUIRE(path.Dirname().String() == "Dir/");
	REQUIRE(path.Filename().String() == "file.cpp");
 }

