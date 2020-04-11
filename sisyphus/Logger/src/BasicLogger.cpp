#include "BasicLogger.h"
#include "Utils/DebugAssert.h"

namespace Sisyphus::Logging {
	BasicLogger::BasicLogger() :
		currentLogLevel(0)
	{

	}

	void BasicLogger::Log(const std::string& message, int logLevel)
	{
		LogInternal(message, logLevel, false);
	}

	void BasicLogger::LogInline(const std::string& message, int logLevel)
	{
		LogInternal(message, logLevel, true);
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

	void BasicLogger::SetLogLevel(int logLevel)
	{
		currentLogLevel = logLevel;
	}

	void BasicLogger::LogInternal(const std::string& message, int logLevel, bool isInline)
	{
		if (logLevel > currentLogLevel) return;

		for (auto&& section : sections) {
			Output(section.indenter);
		}
		Output(message);
		if(!isInline) Output("\n");
	}
}
