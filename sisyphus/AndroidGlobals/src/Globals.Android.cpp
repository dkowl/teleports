#include "Globals.Android.h"
#include "Utils/DebugAssert.h"
#include "Utils/Throw.h"

namespace Sisyphus {
	namespace AndroidGlobals {
		JNIEnv* env = nullptr;
		jobject assetManager = nullptr;
		jstring filesDir = nullptr;

		void Init(JNIEnv* inEnv, jobject inAssetManager) {
			SIS_THROWASSERT(inEnv);
			SIS_THROWASSERT(inAssetManager);
			env = inEnv;
			assetManager = inAssetManager;
		}

		void InitEnv(JNIEnv* inEnv)
		{
			SIS_THROWASSERT(inEnv);
			env = inEnv;
		}

		void InitAssetManager(jobject inAssetManager)
		{
			SIS_THROWASSERT(inAssetManager);
			assetManager = inAssetManager;
		}

		void InitFilesDir(jstring inFilesDir)
		{
			SIS_THROWASSERT(inFilesDir);
			filesDir = inFilesDir;
		}

		JNIEnv* Env() {
			SIS_DEBUGASSERT(env);
			return env;
		}

		jobject AssetManager() {
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