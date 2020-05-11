#include "BasicLogger.h"
#include "Utils/DebugAssert.h"

namespace Sisyphus::Logging {
	BasicLogger::BasicLogger() :
		currentLogLevel(LogLevel::Info)
	{

	}

	void BasicLogger::Log(const std::string& message, LogLevel logLevel)
	{
		if (logLevel > currentLogLevel) return;

		std::string outString;
		for (auto&& section : sections) {
			outString += section.indenter;
		}
		if (!inlineBuffer.empty()) {
			outString += inlineBuffer;
			inlineBuffer = "";
		}
		outString += message;
		Output(outString, logLevel);
	}

	void BasicLogger::LogInline(const std::string& message)
	{
		inlineBuffer += message;
	}

	void BasicLogger::BeginSection(const Section& section) {
		Log(section.Header());
		sections.push_back(section);
	}

	void BasicLogger::BeginSection(const std::string& name)
	{
		Section section(name);
		BeginSection(section);
	}

	void BasicLogger::EndSection()
	{
		SIS_DEBUGASSERT(sections.size() > 0);
		if (sections.size() == 0) return;

		Section section = sections.back();
		sections.pop_back();
		if (!section.Footer().empty()) Log(section.Footer());
	}

	void BasicLogger::SetLogLevel(LogLevel logLevel)
	{
		currentLogLevel = logLevel;
	}
}
