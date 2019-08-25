#pragma once
#include <string>

class ILogger {
public:
	virtual ~ILogger() = default;

	virtual void Log(const std::string& message, int logLevel = 0) = 0;

	virtual void BeginSection(std::string name, std::string indenter = "\t") = 0;
	virtual void EndSection() = 0;

	virtual void SetLogLevel(int logLevel) = 0;
};