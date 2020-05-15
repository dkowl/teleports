#include "BasicLogger.h"
#include "Utils/DebugAssert.h"

namespace Sisyphus::Logging {
	namespace {
		std::string MergeTags(std::string tagA, std::string tagB) {
			if (tagA.empty()) {
				return tagB;
			}
			else if (tagB.empty()) {
				return tagA;
			}
			else{
				return tagA + "-" + tagB;
			}
		}
	}

	BasicLogger::BasicLogger(CreateInfo ci) :
		info(ci),
		currentLogLevel(LogLevel::Info)
	{
		SIS_DEBUGASSERT(info.presenter != nullptr);
	}

	void BasicLogger::Log(const std::string& message, LogLevel logLevel, const std::string& tag)
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
		info.presenter->Present(outString, logLevel, MergeTags(info.tag, tag));
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
