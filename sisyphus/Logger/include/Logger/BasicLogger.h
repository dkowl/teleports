#pragma once
#include "ILogger.h"
#include <vector>
#include <string>

namespace Sisyphus::Logging {
	class BasicLogger : public ILogger {
	public:
		BasicLogger();

		void Log(const std::string& message, LogLevel logLevel = LogLevel::Info) override;
		void LogInline(const std::string& message) override;

		void BeginSection(const Section& section) override;
		void BeginSection(const std::string& name) override;
		void EndSection() override;

		void SetLogLevel(LogLevel logLevel) override;

	protected:
		virtual void Output(const std::string& s, LogLevel logLevel) = 0;

		LogLevel currentLogLevel;
	private:
		std::string inlineBuffer;
		std::vector<Section> sections;
	};
}