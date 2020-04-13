#include "catch.hpp"
#include "Utils/StringUtils.h"

#ifdef SIS_ANDROID
#include "Globals.Android.h"
#include <jni.h>
#include <string>

using namespace JavaGlobals;
#endif

using namespace Sisyphus;

#ifdef SIS_ANDROID
TEST_CASE("StringFromJava") {
	std::string u8str = u8"Zażółć gęślą jaźń";
	std::u16string u16str = u"Zażółć gęślą jaźń";
	jstring jStr = jniEnv->NewString(reinterpret_cast<const jchar*>(u16str.c_str()), u16str.size());

	std::string u8strFromJava = StringFromJava(jniEnv, jStr);
	REQUIRE(u8strFromJava == u8str);

	jStr = jniEnv->NewStringUTF(u8str.c_str());

	u8strFromJava = StringFromJava(jniEnv, jStr);
	REQUIRE(u8str == u8strFromJava);
}
#endif