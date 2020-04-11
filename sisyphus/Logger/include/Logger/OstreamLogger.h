#pragma once
#include "BasicLogger.h"

namespace Sisyphus::Logging {
	class OstreamLogger : public BasicLogger {
	public:
		OstreamLogger(std::ostream& inOstream);
	protected:
		void Output(const std::string& s) override;
	private:
		std::ostream& ostream;
	};
}