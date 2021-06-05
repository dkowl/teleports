#pragma once
#include <jni.h>
#include <android/asset_manager.h>

namespace Sisyphus {
	namespace AndroidGlobals {
		void InitEnv(JNIEnv* inEnv);
		void InitAssetManager(jobject inAssetManager);
		void InitAssetManager(AAssetManager* inAssetManager);
		void InitFilesDir(jstring filesDir);

		JNIEnv* Env();
		AAssetManager* AssetManager();
		jstring FilesDir();
	}
}