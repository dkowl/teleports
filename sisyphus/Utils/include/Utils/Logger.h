#pragma once

#include "ILogger.h"

namespace Logger {
	void Init();
	ILogger& Get();
}