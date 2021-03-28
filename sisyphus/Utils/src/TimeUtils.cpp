#include "TimeUtils.h"
#include "Utils/Platform.h"
#include <ctime>
#include <chrono>
#include <iomanip>
#include <sstream>

namespace Sisyphus {

	std::string CurrentDateStamp() {
		std::time_t time = std::time(nullptr);
		std::tm tm;
#ifdef SIS_WINDOWS
		localtime_s(&tm, &time);
#elif defined(SIS_ANDROID)
		tm = *localtime(&time);
#endif
		std::stringstream sstream;
		sstream << std::put_time(&tm, "%Y-%b-%d_%H-%M-%S");
		return sstream.str();
	}

	uint64_t MicrosecondsSinceEpoch() {
		return std::chrono::duration_cast<std::chrono::microseconds>(std::chrono::system_clock::now().time_since_epoch()).count();
	}
}