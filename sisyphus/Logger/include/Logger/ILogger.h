#pragma once
#include <string>
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

		virtual void Log(const std::string& message, int logLevel = 0) = 0;
		virtual void LogInline(const std::string& message, int logLevel = 0) = 0;

		virtual void BeginSection(const Section& section) = 0;
		virtual void BeginSection(const std::string& name) = 0;
		virtual void EndSection() = 0;

		virtual void SetLogLevel(int logLevel) = 0;
	};
}