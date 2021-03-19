#include "catch.hpp"
#include "Path.h"
#include "Utils/Platform.h"

using namespace Sisyphus::Fs;

TEST_CASE("Path") {
	Path path("Dir/Dir2/file.cpp");

	REQUIRE(path.Stem().String() == "file");
	REQUIRE(path.Filename().String() == "file.cpp");
	REQUIRE(path.Dirname().String() == "Dir/Dir2/");
	REQUIRE(path.Extension().String() == ".cpp");
	REQUIRE(path.LastSegment() == "file.cpp");

	path = "file.h"; // testing operator=
	REQUIRE(path.Extension().String() != ".cpp");
	REQUIRE(path.Extension().String() == ".h");

	Path dir("Dir/Dir2/");
	REQUIRE(dir.LastSegment() == "Dir2");

	Path emptyPath("");
	REQUIRE(emptyPath.Filename().String().empty());
	REQUIRE(emptyPath.LastSegment() == "");

	Path defaultPath;
	REQUIRE(defaultPath.Filename().String().empty());
	REQUIRE(defaultPath.LastSegment() == "");
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

	REQUIRE(path.Dirname().String().substr(0, 3) == "Dir");
	REQUIRE(Path::IsSeparator(path.Dirname().CStr() + 3));
	REQUIRE(path.Filename().String() == "file.cpp");
 }

TEST_CASE("Path - file with no extension") {
	Path path("dir/file");
	REQUIRE(path.Extension().String() == "");
	REQUIRE(path.Stem().String() == "file");

	Path emptyPath;
	REQUIRE(emptyPath.Filename().String().empty());
	REQUIRE(emptyPath.Dirname().String().empty());
}

