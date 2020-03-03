#include "Filesystem.h"
#include "android/asset_manager.h"
#include "android/asset_manager_jni.h"

namespace Sisyphus::Fs {

	AAssetManager* assetManager = nullptr;

	void Init_Android(void* env, void* javaAssetManager) {
		assetManager = AAssetManager_fromJava((JNIEnv*)env, (jobject)javaAssetManager);
	}

	bool Exists(const Path& p) {
		if (IsRegularFile(p)) return true;
		else if (IsDirectory(p)) return true;
		else return false;
	}

	bool IsRegularFile(const Path& p) {
		auto asset = AAssetManager_open(assetManager, p.String().c_str(), AASSET_MODE_UNKNOWN);
		if (asset == nullptr) return false;
		else {
			AAsset_close(asset);
			return true;
		}
	}

	bool IsDirectory(const Path& p) {
		auto assetDir = AAssetManager_openDir(assetManager, p.String().c_str());
		if (assetDir == nullptr) return false;
		else {
			AAssetDir_close(assetDir);
			return true;
		}
	}


}