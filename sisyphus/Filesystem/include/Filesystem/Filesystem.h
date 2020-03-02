#include "Path.h"
#include "Utils/PlatformMacros.h"

namespace Sisyphus::Fs {

#ifdef SIS_ANDROID
	void Init_Android(void* env, void* javaAssetManager);
#endif

	bool Exists(const Path& p);
}