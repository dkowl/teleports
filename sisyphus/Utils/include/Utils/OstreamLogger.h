#pragma once
#include "ILogger.h"
#include <ostream>
#include <vector>
#include <string>

class OstreamLogger : public ILogger {
public:
	OstreamLogger();

	void Log(const std::string& message, int logLevel = 0) override;

	void BeginSection(std::string name, std::string indenter) override;
	void EndSection() override;

	void SetLogLevel(int logLevel) override;

private:
	struct Section {
		std::string name;
		std::string indenter;

		std::string Header() const;
		std::string Footer() const;
	};

	std::ostream* output;
	std::vector<Section> sections;
	int currentLogLevel;
};