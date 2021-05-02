#pragma once

#ifdef _WIN32
#define SIS_WINDOWS
#endif

#ifdef __ANDROID__
#define SIS_ANDROID
#endif

#include <string>

namespace Sisyphus {

	enum class Platform {
		Unknown,
		Windows,
		Android
	};

	const char* PlatformAsString(Platform platform);
	Platform PlatformFromString(const std::string& s);
	Platform CurrentPlatform();
}