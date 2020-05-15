#include "Logger.h"
#include "BasicLogger.h"
#include "OstreamLogPresenter.h"
#include "AndroidLogPresenter.h"
#include "Utils/PlatformMacros.h"
#include <iostream>

namespace Sisyphus {

	using namespace Logging;

	namespace {
		std::unique_ptr<LogPresenter> presenter = nullptr;
		std::unique_ptr<ILogger> logger = nullptr;

		void InitPresenter() {
#ifdef SIS_ANDROID
			presenter = std::make_unique<AndroidLogPresenter>();
#else
			presenter = std::make_unique<OstreamLogPresenter>(std::cout);
#endif
		}

		void InitLogger() {
			BasicLogger::CreateInfo createInfo;
			createInfo.presenter = &DefaultLogPresenter();
			logger = std::make_unique<BasicLogger>(createInfo);
		}
	}

	ILogger& Logger() {
		if (logger == nullptr) {
			InitLogger();
		}
		return *logger;
	}

	Logging::LogPresenter& DefaultLogPresenter()
	{
		if (presenter == nullptr) {
			InitPresenter();
		}
		return *presenter;
	}
}