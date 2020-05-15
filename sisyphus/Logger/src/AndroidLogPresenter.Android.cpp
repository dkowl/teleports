#include "AndroidLogPresenter.h"
#include <android/log.h>
#include <unordered_map>

namespace Sisyphus::Logging {

	namespace {
		int AndroidLogLevel(LogLevel lvl) {
			static std::unordered_map<LogLevel, int> map{
				{LogLevel::All, ANDROID_LOG_UNKNOWN},
				{LogLevel::Trace, ANDROID_LOG_VERBOSE},
				{LogLevel::Debug, ANDROID_LOG_DEBUG},
				{LogLevel::Default, ANDROID_LOG_DEFAULT},
				{LogLevel::Info, ANDROID_LOG_INFO},
				{LogLevel::Warn, ANDROID_LOG_WARN},
				{LogLevel::Error, ANDROID_LOG_ERROR},
				{LogLevel::Fatal, ANDROID_LOG_FATAL},
				{LogLevel::None, ANDROID_LOG_SILENT}
			};

			return map[lvl];
		}
	}

	void AndroidLogPresenter::Present(const std::string& s, LogLevel logLevel, const std::string& tag) {
		std::string finalTag = "Sisyhpus";
		if (!tag.empty()) {
			finalTag += "-" + tag;
		}
		__android_log_write(AndroidLogLevel(logLevel), finalTag.c_str(), s.c_str());
	}
}