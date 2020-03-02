#include "catch.hpp"
#include "Path.h"

using namespace Sisyphus::Filesystem;

TEST_CASE("Stem") {
	Path path("Dir/Dir2/file.cpp");

	REQUIRE(path.Stem().String() == "file");
	REQUIRE(path.Filename().String() == "file.cpp");
	REQUIRE(path.Dirname().String() == "Dir/Dir2/");
	REQUIRE(path.Extension().String() == ".cpp");
}
