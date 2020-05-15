#pragma once
#include "LogPresenter.h"

namespace Sisyphus::Logging {
	class OstreamLogPresenter : public LogPresenter {
	public:
		OstreamLogPresenter(std::ostream& inOstream);
	protected:
		void Present(const std::string& s, LogLevel logLevel, const std::string& tag) override;
	private:
		std::ostream& ostream;
	};
}