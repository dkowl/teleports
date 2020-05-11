#pragma once
#include "ILogger.h"
#include <vector>
#include <string>

namespace Sisyphus::Logging {
	class BasicLogger : public ILogger {
	public:
		BasicLogger();

		void Log(const std::string& message, LogLevel logLevel = LogLevel::Info) override;
		void LogInline(const std::string& message, LogLevel logLevel = LogLevel::Info) override;

		void BeginSection(const Section& section) override;
		void BeginSection(const std::string& name) override;
		void EndSection() override;

		void SetLogLevel(LogLevel logLevel) override;

	protected:
		virtual void Output(const std::string& s) = 0;

		LogLevel currentLogLevel;
	private:
		void LogInternal(const std::string& message, LogLevel logLevel, bool isInline);

		std::vector<Section> sections;
	};
}