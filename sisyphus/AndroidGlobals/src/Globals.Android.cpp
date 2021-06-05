#include "Globals.Android.h"
#include "Utils/DebugAssert.h"
#include "Utils/Throw.h"
#include <android/asset_manager_jni.h>

namespace Sisyphus {
	namespace AndroidGlobals {
		JNIEnv* env = nullptr;
		AAssetManager* assetManager = nullptr;
		jstring filesDir = nullptr;

		void InitEnv(JNIEnv* inEnv)
		{
			SIS_THROWASSERT(!env && inEnv);
			env = inEnv;
		}

		void InitAssetManager(jobject inAssetManager)
		{
			SIS_THROWASSERT(!assetManager && inAssetManager); 
			assetManager = AAssetManager_fromJava(Env(), inAssetManager);;
		}

		void InitAssetManager(AAssetManager* inAssetManager)
		{
			SIS_THROWASSERT(!assetManager && inAssetManager);
			assetManager = inAssetManager;
		}

		void InitFilesDir(jstring inFilesDir)
		{
			SIS_THROWASSERT(!filesDir && inFilesDir);
			filesDir = inFilesDir;
		}

		JNIEnv* Env() {
			SIS_DEBUGASSERT(env);
			return env;
		}

		AAssetManager* AssetManager() {
			SIS_DEBUGASSERT(assetManager);
			return assetManager;
		}

		jstring FilesDir()
		{
			SIS_DEBUGASSERT(filesDir);
			return filesDir;
		}
	}
}