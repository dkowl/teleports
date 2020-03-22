#include "Pch_Utils.h"
#include "Logger.h"
#include "OstreamLogger.h"


namespace Sisyphus {

	namespace {
		std::unique_ptr<ILogger> logger = nullptr;

		void Init() {
			logger = std::make_unique<OstreamLogger>();
		}
	}

	ILogger& Logger() {
		if (logger == nullptr) {
			Init();
		}
		return *logger;
	}
}