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

		void BeginSection(std::string name, std::string indenter = "\t") override;
		void EndSection() override;

		void SetLogLevel(int logLevel) override;

	protected:
		virtual void Output(const std::string& s) = 0;

		int currentLogLevel;
	private:
		void LogInternal(const std::string& message, int logLevel, bool isInline);

		struct Section {
			std::string name;
			std::string indenter;

			std::string Header() const;
			std::string Footer() const;
		};

		std::vector<Section> sections;
	};
}