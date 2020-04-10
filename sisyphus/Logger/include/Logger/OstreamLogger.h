#pragma once
#include "BasicLogger.h"

namespace Sisyphus::Logging {
	class OstreamLogger : public BasicLogger {
	protected:
		void Output(const std::string& s) override;
	};
}