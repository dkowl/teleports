#include "OstreamLogPresenter.h"
#include "Utils/DebugAssert.h"
#include <ostream>

namespace Sisyphus::Logging {
	OstreamLogPresenter::OstreamLogPresenter(std::ostream& inOstream):
		ostream(inOstream)
	{}

	void OstreamLogPresenter::Present(const std::string& s, LogLevel, const std::string&) {
		ostream << s << "\n";
	}
}
