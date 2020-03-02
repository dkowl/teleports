#include "catch.hpp"
#include "Filesystem.h"
#include "Path.h"
#include "Utils/PlatformMacros.h"

using namespace Sisyphus;

#ifdef SIS_ANDROID
#include "Globals.Android.h"
#endif

TEST_CASE("Exists") {
	Fs::Path dummyPath("dummy.txt");

#ifdef SIS_ANDROID
	Fs::Init_Android((void*)JavaGlobals::jniEnv, (void*)JavaGlobals::assetManager);
#endif

	REQUIRE(!Fs::Exists(dummyPath));
}