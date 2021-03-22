#include "TimeUtils.h"
#include <ctime>
#include <chrono>
#include <iomanip>
#include <sstream>

namespace Sisyphus {

	std::string CurrentDateStamp() {
		std::time_t time = std::time(nullptr);
		std::tm tm;
		localtime_s(&tm, &time);
		std::stringstream sstream;
		sstream << std::put_time(&tm, "%Y-%b-%d_%H-%M-%S");
		return sstream.str();
	}

	uint64_t MicrosecondsSinceEpoch() {
		return std::chrono::duration_cast<std::chrono::microseconds>(std::chrono::system_clock::now().time_since_epoch()).count();
	}
}