#pragma once
#include "ILogger.h"
#include <vector>
#include <string>

namespace Sisyphus::Logging {
	class BasicLogger : public ILogger {
	public:
		BasicLogger();

		void Log(const std::string& message, int logLevel = 0) override;
		void LogInline(const std::string& message, int logLevel = 0) override;

		void BeginSection(const Section& section) override;
		void BeginSection(const std::string& name) override;
		void EndSection() override;

		void SetLogLevel(int logLevel) override;

	protected:
		virtual void Output(const std::string& s) = 0;

		int currentLogLevel;
	private:
		void LogInternal(const std::string& message, int logLevel, bool isInline);

		std::vector<Section> sections;
	};
}