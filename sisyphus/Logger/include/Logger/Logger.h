#pragma once

#include "ILogger.h"
#include "LogPresenter.h"
#include "LogLevel.h"

namespace Sisyphus {
	// this function is to be defined in user code
	// e.g. AssetManagement defines this to return a Logger tagged with "AssetManagement"
	// however, all projects can still just call "Logger()" and not care about it
	Logging::ILogger& Logger();

	Logging::ILogger& DefaultLogger();

	Logging::LogPresenter& DefaultLogPresenter();
}