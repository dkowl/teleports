#include "catch.hpp"
#include "Filesystem.h"
#include "Path.h"
#include "Utils/PlatformMacros.h"
#include <fstream>
#include <iostream>
#include <cstdio>
#include <set>

using namespace Sisyphus;

#ifdef SIS_WINDOWS
#include <filesystem>
#endif

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
	catch (...) {

	}

	file.close();
	if (remove(dummyPath.String().c_str()) != 0) {
		std::cout << "Error deleting file " << dummyPath.String() << "\n";
	}
	else {
		std::cout << "File " << dummyPath.String() << " deleted.\n";
	}
}

TEST_CASE("RecursiveDirectoryIterator"){
#ifdef SIS_WINDOWS
	std::string dirName = "dir";
	std::filesystem::create_directory(dirName);
	std::cout << "Directory " << dirName << " created\n";

	try {
		std::fstream file1(dirName + "/file1.txt", std::fstream::out);
		std::fstream file2(dirName + "/file2.txt", std::fstream::out);

		std::string nestedDirName = "dir2";
		std::filesystem::create_directory(dirName + "/" + nestedDirName);
		std::cout << "Directory " << nestedDirName << " created\n";

		std::fstream file3(dirName + "/" + nestedDirName + "/file3.txt", std::fstream::out);

		std::set<std::string> seenFilenames;
		int numEntries = 0;
		for (auto&& p : Fs::RecursiveDirectoryIterator(Fs::Path("dir"))) {
			seenFilenames.insert(p.Filename().String());
			numEntries++;
		}

		REQUIRE(seenFilenames.contains("file1.txt"));
		REQUIRE(seenFilenames.contains("file2.txt"));
		REQUIRE(seenFilenames.contains("file3.txt"));
		REQUIRE(numEntries == 4);		
	}
	catch (...) {

	}

	std::filesystem::remove_all(dirName);
	std::cout << "Directory " << dirName << " deleted with its content\n";
#endif
}