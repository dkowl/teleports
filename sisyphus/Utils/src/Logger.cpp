#include "Logger.h"
#include "OstreamLogger.h"
#include <memory>

namespace Logger {
	std::unique_ptr<ILogger> logger = nullptr;

	void Init() {
		logger = std::make_unique<OstreamLogger>();
	}

	ILogger& Get() {
		if (logger == nullptr) {
			Init();
		}
		return *logger;
	}
}