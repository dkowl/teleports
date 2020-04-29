#include "catch.hpp"
#include "Filesystem.h"
#include "Path.h"
#include "Utils/PlatformMacros.h"
#include "Utils/StringUtils.h"
#include "Logger/Logger.h"
#include <fstream>
#include <cstdio>
#include <set>

using namespace Sisyphus;
using namespace Sisyphus::Logging;

#ifdef SIS_WINDOWS
#include <filesystem>
#endif

#ifdef SIS_ANDROID
#include "AndroidGlobals/Globals.Android.h"
#endif

TEST_CASE("Filesystem") {
	Fs::Path workingDir = std::string("./");

#ifdef SIS_ANDROID
	workingDir = StringFromJava(AndroidGlobals::Env(), AndroidGlobals::FilesDir());
#endif

	Fs::Path filename = "dummy.txt";
	Fs::Path dummyPath = workingDir / filename;

	REQUIRE(!Fs::Exists(dummyPath));
	REQUIRE(!Fs::IsRegularFile(dummyPath));
	REQUIRE(!Fs::IsDirectory(dummyPath));

	std::fstream file(dummyPath.String(), std::fstream::out | std::fstream::binary);
	if (file.good()) {
		Logger().Log(AssembleString("File ", dummyPath.String(), " opened.\n"));
	}
	else {
		Logger().Log(AssembleString("Error opening file ", dummyPath.String(),  "\n"));
		REQUIRE(false);
	}

	try {
		REQUIRE(Fs::Exists(dummyPath));
		REQUIRE(Fs::IsRegularFile(dummyPath));
		REQUIRE(!Fs::IsDirectory(dummyPath));

		REQUIRE(Fs::FileSize(dummyPath) == 0);
		int numbers[] = { 1 , 2, 3 };
		file.write(reinterpret_cast<const char*>(numbers), sizeof(numbers));
		file.flush();
		REQUIRE(Fs::FileSize(dummyPath) == 3 * sizeof(int));
	}
	catch (...) {

	}

	file.close();
	if (remove(dummyPath.String().c_str()) != 0) {
		Logger().Log(AssembleString("Error deleting file ", dummyPath.String(), "\n"));
	}
	else {
		Logger().Log(AssembleString("File ", dummyPath.String(), " deleted.\n"));
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
	// TODO: Android
}