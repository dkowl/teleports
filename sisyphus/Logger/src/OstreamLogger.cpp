#include "OstreamLogger.h"
#include "Utils/DebugAssert.h"
#include <iostream>

namespace Sisyphus::Logging {
	void OstreamLogger::Output(const std::string& s) {
		std::cout << s;
	}
}
