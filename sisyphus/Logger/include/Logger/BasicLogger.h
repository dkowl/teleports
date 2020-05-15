#pragma once
#include "ILogger.h"
#include "LogPresenter.h"
#include <vector>
#include <string>

namespace Sisyphus::Logging {
	class BasicLogger : public ILogger {
	public:
		struct CreateInfo {
			LogPresenter* presenter = nullptr;
			std::string tag = "";
		};

		BasicLogger(CreateInfo ci);

		void Log(
			const std::string& message, 
			LogLevel logLevel = LogLevel::Info, 
			const std::string& tag = "") override;
		void LogInline(const std::string& message) override;

		void BeginSection(const Section& section) override;
		void BeginSection(const std::string& name) override;
		void EndSection() override;

		void SetLogLevel(LogLevel logLevel) override;

	private:
		CreateInfo info;
		std::string inlineBuffer;
		std::vector<Section> sections;
		LogLevel currentLogLevel;
	};
}