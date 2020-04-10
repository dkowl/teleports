#include "AndroidLogger.h"
#include <android/log.h>

namespace Sisyphus::Logging {

	void AndroidLogger::Output(const std::string& s) {
		// WISH: map logLevels to interal Android log levels from https://developer.android.com/ndk/reference/group/logging
		__android_log_write(currentLogLevel, "Sisyphus", s.c_str());
	}
}