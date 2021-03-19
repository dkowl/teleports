#pragma once

#include "Utils/PlatformMacros.h"

namespace Sisyphus {

	enum class Platform {
		Windows,
		Android
	};

	const char* PlatformAsString(Platform platform);
	Platform CurrentPlatform();
}