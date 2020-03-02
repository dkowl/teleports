#include "Filesystem.h"
#include "android/asset_manager.h"
#include "android/asset_manager_jni.h"

namespace Sisyphus::Fs {

	AAssetManager* assetManager = nullptr;

	void Init_Android(void* env, void* javaAssetManager) {
		assetManager = AAssetManager_fromJava((JNIEnv*)env, (jobject)javaAssetManager);
	}

	bool Exists(const Path& p) {
		auto asset = AAssetManager_open(assetManager, p.String().c_str(), AASSET_MODE_UNKNOWN);
		return asset != nullptr;
	}
}