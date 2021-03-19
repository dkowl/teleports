#include "Platform.h"
#include "DebugAssert.h"

namespace Sisyphus{
	const char* PlatformAsString(Platform platform) {
		switch (platform) {
		case Platform::Windows:
			return "Windows";
		case Platform::Android:
			return "Android";
		default:
			SIS_DEBUGASSERT_MSG(false, "Unknown platform");
			return "Unknown platform";
		}
	}

	Platform CurrentPlatform() {
#ifdef SIS_WINDOWS
		return Platform::Windows;
#elif SIS_ANDROID
		return Platform::Android;
#endif
	}
}
