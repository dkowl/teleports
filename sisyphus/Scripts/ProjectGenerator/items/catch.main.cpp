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
jstring JavaGlobals::filesDir = nullptr;

extern "C"
JNIEXPORT JNICALL
int Java_com_SIS_REPLACE(APPNAME)_SIS_REPLACE(APPNAME)_runTest(JNIEnv * env, jclass type, jobject assetManager, jstring filesDir) {
    JavaGlobals::jniEnv = env;
    JavaGlobals::assetManager = assetManager;
    JavaGlobals::filesDir = filesDir;
    const char* argv[] = { "whatever" };
    int result = Catch::Session().run(1, argv);//fake `argc` and `argv`
    return result;
}
#endif