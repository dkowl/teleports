#include "Filesystem.h"
#include "android/asset_manager.h"
#include "android/asset_manager_jni.h"
#include "Utils/Throw.h"
#include "Logger/Logger.h"
#include <vector>

namespace Sisyphus::Fs {

	AAssetManager* assetManager = nullptr;

	void Init_Android(void* env, void* javaAssetManager) {
		if (env == nullptr) {
			Logger().Log("JNIEnv is null!");
			return;
		}
		if(javaAssetManager == nullptr){
			Logger().Log("javaAssetManager is null!");
			return;
		}
		try {
			Logger().Log("Calling Init_Android");
			assetManager = AAssetManager_fromJava((JNIEnv*)env, (jobject)javaAssetManager);
		}
		catch (...) {
			Logger().Log("Init_Android FAILED!");
		}
		Logger().Log("Android filesystem initialized");
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
		// https://stackoverflow.com/questions/26101371/checking-if-directory-folder-exists-in-apk-via-native-code-only
		auto assetDir = AAssetManager_openDir(assetManager, p.String().c_str());
		bool openSuccessful = AAssetDir_getNextFileName(assetDir) != nullptr;
		AAssetDir_close(assetDir);
		return openSuccessful;
	}

	uint64_t FileSize(const Path& p) {
		auto asset = AAssetManager_open(assetManager, p.String().c_str(), AASSET_MODE_UNKNOWN);
		if (asset == nullptr) return 0;
		else {
			uint64_t length = AAsset_getLength64(asset);
			AAsset_close(asset);
			return length;
		}
	}

	class RecursiveDirectoryIterator::Impl {
	public:
		Impl(const Path& p) {
			auto assetDir = AAssetManager_openDir(assetManager, p.String().c_str());
			SIS_THROWASSERT_MSG(assetDir != nullptr, "Failed to open asset directory: " + p.String());
			assetDirs.push_back(assetDir);

			Increment();
		}

		void Increment() {
			// Recursive alogrithm	
			const char* nextFilename = nullptr;
			while (!assetDirs.empty()) {
				auto nextFilename = AAssetDir_getNextFileName(assetDirs.back());
				if (nextFilename == nullptr) {
					assetDirs.pop_back();
				}
				else {
					break;
				}
			}

			AddIfDir(nextFilename);

			currentPath = nextFilename == nullptr ? nextFilename : "";
		}

		std::vector<AAssetDir*> assetDirs;
		Path currentPath;

	private:
		void AddIfDir(const char* dirName) {
			AAssetDir* assetDir = nullptr;
			bool failed = false;
			try {
				assetDir = AAssetManager_openDir(assetManager, dirName);
			}
			catch (...) {
				failed = true;
			}
			if (!failed && assetDir != nullptr) {
				assetDirs.push_back(assetDir);
			}
		}
	};

	RecursiveDirectoryIterator::RecursiveDirectoryIterator(const Path& p) :
		impl(std::make_unique<Impl>(p))
	{
	}

	RecursiveDirectoryIterator::~RecursiveDirectoryIterator() = default;

	RecursiveDirectoryIterator::RecursiveDirectoryIterator(const RecursiveDirectoryIterator& other)
	{
		impl = std::make_unique<Impl>(*(other.impl));
	}

	const Path& RecursiveDirectoryIterator::operator*() const
	{
		return impl->currentPath;
	}
	const Path* RecursiveDirectoryIterator::operator->() const
	{
		return &(impl->currentPath);
	}

	RecursiveDirectoryIterator& RecursiveDirectoryIterator::operator++()
	{
		impl->Increment();
		return *this;
	}
	RecursiveDirectoryIterator& RecursiveDirectoryIterator::begin()
	{
		return *this;
	}
	RecursiveDirectoryIterator::End RecursiveDirectoryIterator::end() const
	{
		return End();
	}
	bool RecursiveDirectoryIterator::operator!=(End)
	{
		return impl->currentPath.Empty();
	}
}