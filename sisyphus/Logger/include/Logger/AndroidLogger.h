#include "BasicLogger.h"

namespace Sisyphus::Logging {
	class AndroidLogger : public BasicLogger {
	protected:
		void Output(const std::string& s, LogLevel logLevel) override;
	};
}