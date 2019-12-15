#pragma once
#include <string>
#include "Utils/StringUtils.h"

class ILogger {
public:
	virtual ~ILogger() = default;

	template<typename... Args>
	void LogArgs(Args... args) {
		Log(Sisyphus::AssembleString(std::forward(args...)));
	}

	virtual void Log(const std::string& message, int logLevel = 0) = 0;
	
	virtual void BeginSection(std::string name, std::string indenter = "\t") = 0;
	virtual void EndSection() = 0;

	virtual void SetLogLevel(int logLevel) = 0;
};