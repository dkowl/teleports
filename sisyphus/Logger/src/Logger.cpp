#include "Logger.h"
#include "OstreamLogger.h"
#include "AndroidLogger.h"
#include "Utils/PlatformMacros.h"
#include <iostream>

namespace Sisyphus {

	using namespace Logging;

	namespace {
		std::unique_ptr<ILogger> logger = nullptr;

		void Init() {
#ifdef SIS_ANDROID
			logger = std::make_unique<AndroidLogger>();
#else
			logger = std::make_unique<OstreamLogger>(std::cout);
#endif
		}
	}

	ILogger& Logger() {
		if (logger == nullptr) {
			Init();
		}
		return *logger;
	}
}