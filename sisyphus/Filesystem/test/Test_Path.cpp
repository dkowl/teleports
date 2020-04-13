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

	// operator/=
	path = "Dir";
	path /= Path("file.cpp");
	REQUIRE(path.Dirname().String() == "Dir/");
	REQUIRE(path.Filename().String() == "file.cpp");
	path = "Dir";
	path /= "file.cpp";
	REQUIRE(path.Dirname().String() == "Dir/");
	REQUIRE(path.Filename().String() == "file.cpp");
	
}
