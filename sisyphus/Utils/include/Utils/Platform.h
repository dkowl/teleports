#pragma once

#ifdef _WIN32
#define SIS_WINDOWS
#endif

#ifdef __ANDROID__
#define SIS_ANDROID
#endif

#ifdef __clang__
#define SIS_CLANG
#endif

namespace Sisyphus {

	enum class Platform {
		Windows,
		Android
	};

	const char* PlatformAsString(Platform platform);
	Platform CurrentPlatform();
}