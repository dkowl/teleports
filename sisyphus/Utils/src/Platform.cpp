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

	Platform PlatformFromString(const std::string& s) {
		if (s == "Windows") {
			return Platform::Windows;
		}
		else if (s == "Android") {
			return Platform::Android;
		}
		else {
			SIS_DEBUGASSERT_MSG(false, "Unknown platform name: " + s);
			return Platform::Unknown;
		}
	}

	Platform CurrentPlatform() {
#ifdef SIS_WINDOWS
		return Platform::Windows;
#elif defined(SIS_ANDROID)
		return Platform::Android;
#endif
	}
}
