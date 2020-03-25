#pragma once
#include <jni.h>

namespace JavaGlobals {
	extern JNIEnv* jniEnv;
	extern jobject assetManager;
	extern jstring filesDir;
}