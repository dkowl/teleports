#ifdef __ANDROID__
#define CATCH_CONFIG_RUNNER
#else
#define CATCH_CONFIG_MAIN
#endif
#include "catch.hpp"

#ifdef __ANDROID__
#include <jni.h>
#include "Globals.Android.h"

JNIEnv* JavaGlobals::jniEnv = nullptr;
jobject JavaGlobals::assetManager = nullptr;

extern "C"
JNIEXPORT JNICALL
int Java_com_UtilsAndroidTestApp_UtilsAndroidTestApp_runTest(JNIEnv * env, jclass type, jobject assetManager) {
	JavaGlobals::jniEnv = env;
    JavaGlobals::assetManager = assetManager;
    const char* argv[] = { "whatever" };
    int result = Catch::Session().run(1, argv);//fake `argc` and `argv`
    return result;
}
#endif