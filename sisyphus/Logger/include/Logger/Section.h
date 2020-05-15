#pragma once
#include <string>
#include "LogLevel.h"

namespace Sisyphus::Logging {
	struct Section {
		Section(const std::string& inName, LogLevel inLogLevel = LogLevel::Default, const std::string& inTag = "");

		std::string name;
		std::string indenter;
		std::string leftHeaderDecorator;
		std::string rightHeaderDecorator;
		std::string footer;
		LogLevel logLevel;
		std::string tag;

		std::string Header() const;
		std::string Footer() const;
	};
}