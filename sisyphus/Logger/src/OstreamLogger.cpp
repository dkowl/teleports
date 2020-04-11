#include "OstreamLogger.h"
#include "Utils/DebugAssert.h"
#include <ostream>

namespace Sisyphus::Logging {
	OstreamLogger::OstreamLogger(std::ostream& inOstream):
		ostream(inOstream)
	{}

	void OstreamLogger::Output(const std::string& s) {
		ostream << s;
	}
}
