#include "LogPresenter.h"

namespace Sisyphus::Logging {
	class AndroidLogPresenter : public LogPresenter {
	protected:
		void Present(const std::string& s, LogLevel logLevel, const std::string& tag) override;
	};
}