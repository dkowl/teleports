#ifdef __ANDROID__
#define CATCH_CONFIG_RUNNER
#else
#define CATCH_CONFIG_MAIN
#endif
#include "catch.hpp"
#include "catch.globals.h"

#ifdef __ANDROID__
#include <jni.h>
#include "Globals.Android.h"

JNIEnv* JavaGlobals::jniEnv = nullptr;
jobject JavaGlobals::assetManager = nullptr;
jstring JavaGlobals::filesDir = nullptr;

extern "C"
JNIEXPORT JNICALL
jstring Java_com_SIS_REPLACE(APPNAME)_SIS_REPLACE(APPNAME)_runTest(JNIEnv * env, jclass type, jobject assetManager, jstring filesDir) {
    JavaGlobals::jniEnv = env;
    JavaGlobals::assetManager = assetManager;
    JavaGlobals::filesDir = filesDir;
    const char* argv[] = { "SIS_REPLACE(PROJNAME)" };
    Catch::Session().run(1, argv);
    return env->NewStringUTF(CatchGlobals::listenerOutput.c_str());
}
#endif