#pragma once
#include <jni.h>

namespace Sisyphus {
	namespace AndroidGlobals {
		void InitEnv(JNIEnv* inEnv);
		void InitAssetManager(jobject inAssetManager);
		void InitFilesDir(jstring filesDir);

		JNIEnv* Env();
		jobject AssetManager();
		jstring FilesDir();
	}
}