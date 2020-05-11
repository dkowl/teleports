#pragma once
#include <string>
#include "Logger/LogLevel.h"
#include "Logger/Section.h"
#include "Utils/StringUtils.h"

namespace Sisyphus::Logging {
	class ILogger {
	public:
		virtual ~ILogger() = default;

		template<typename... Args>
		void LogArgs(Args&&... args) {
			Log(Sisyphus::AssembleString(std::forward<Args>(args)...));
		}

		virtual void Log(const std::string& message, LogLevel logLevel = LogLevel::Info) = 0;
		virtual void LogInline(const std::string& message) = 0;

		virtual void BeginSection(const Section& section) = 0;
		virtual void BeginSection(const std::string& name) = 0;
		virtual void EndSection() = 0;

		virtual void SetLogLevel(LogLevel logLevel) = 0;
	};
}