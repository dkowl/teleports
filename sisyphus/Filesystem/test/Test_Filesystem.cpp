#include "catch.hpp"
#include "Filesystem.h"
#include "Path.h"
#include "Utils/PlatformMacros.h"
#include <fstream>
#include <iostream>
#include <cstdio>

using namespace Sisyphus;

#ifdef SIS_ANDROID
#include "Globals.Android.h"
#endif

TEST_CASE("Filesystem") {
#ifdef SIS_ANDROID
	Fs::Init_Android((void*)JavaGlobals::jniEnv, (void*)JavaGlobals::assetManager);
#endif

	Fs::Path dummyPath("dummy.txt");

	REQUIRE(!Fs::Exists(dummyPath));
	REQUIRE(!Fs::IsRegularFile(dummyPath));
	REQUIRE(!Fs::IsDirectory(dummyPath));

	std::fstream file(dummyPath.String(), std::fstream::out);
	if (file.good()) {
		std::cout << "File " << dummyPath.String() << " opened.\n";
	}
	else {
		std::cout << "Error opening file " << dummyPath.String() << "\n";
		REQUIRE(false);
	}
	
	try {
		REQUIRE(Fs::Exists(dummyPath));
		REQUIRE(Fs::IsRegularFile(dummyPath));
		REQUIRE(!Fs::IsDirectory(dummyPath));
	}
	catch(...){

	}

	file.close();
	if( remove(dummyPath.String().c_str()) != 0 ){
		std::cout << "Error deleting file " << dummyPath.String() << "\n";
	}
	else {
		std::cout << "File " << dummyPath.String() << " deleted.\n";
	}
}