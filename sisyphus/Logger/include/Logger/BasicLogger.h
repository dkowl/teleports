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
			LogLevel logLevel = LogLevel::Default, 
			const std::string& tag = ""
		) override;
		void LogInline(const std::string& message) override;

		void BeginSection(const Section& section) override;
		void BeginSection(
			const std::string& name, 
			LogLevel logLevel = LogLevel::Default,
			const std::string& tag = ""
		) override;
		void EndSection() override;

		void SetLogLevel(LogLevel logLevel) override;

	private:
		bool ShouldSkipLog(LogLevel logLevel) const;
		std::string FinalTag(std::string tag) const;

		CreateInfo info;
		std::string inlineBuffer;
		std::vector<Section> sections;
		LogLevel currentLogLevel;
	};
}