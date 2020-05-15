#pragma once

#include "ILogger.h"
#include "LogPresenter.h"
#include "LogLevel.h"

namespace Sisyphus {
	Logging::ILogger& Logger();

	Logging::LogPresenter& DefaultLogPresenter();
}