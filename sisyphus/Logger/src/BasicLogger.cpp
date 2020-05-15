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
		currentLogLevel(LogLevel::Default)
	{
		SIS_DEBUGASSERT(info.presenter != nullptr);
	}

	void BasicLogger::Log(const std::string& message, LogLevel logLevel, const std::string& tag)
	{
		if (ShouldSkipLog(logLevel)) return;

		std::string outString;
		for (auto&& section : sections) {
			outString += section.indenter;
		}
		if (!inlineBuffer.empty()) {
			outString += inlineBuffer;
			inlineBuffer = "";
		}
		outString += message;
		info.presenter->Present(outString, logLevel, FinalTag(tag));
	}

	void BasicLogger::LogInline(const std::string& message)
	{
		inlineBuffer += message;
	}

	void BasicLogger::BeginSection(const Section& section) {
		Log(section.Header(), section.logLevel, section.tag);
		sections.push_back(section);
	}

	void BasicLogger::BeginSection(const std::string& name, LogLevel logLevel, const std::string& tag)
	{
		Section section(name, logLevel, tag);
		BeginSection(section);
	}

	void BasicLogger::EndSection()
	{
		SIS_DEBUGASSERT(sections.size() > 0);
		if (sections.size() == 0) return;

		Section section = sections.back();
		sections.pop_back();
		if (!section.Footer().empty()) Log(section.Footer(), section.logLevel, section.tag);
	}

	void BasicLogger::SetLogLevel(LogLevel logLevel)
	{
		currentLogLevel = logLevel;
	}
	bool BasicLogger::ShouldSkipLog(LogLevel logLevel) const
	{
		if (logLevel == LogLevel::Default && !sections.empty()) {
			logLevel = sections.back().logLevel;
		}
		return logLevel > currentLogLevel;
	}
	std::string BasicLogger::FinalTag(std::string tag) const
	{
		if (tag == "" && !sections.empty()) {
			tag = sections.back().tag;
		}
		return MergeTags(info.tag, tag);
	}
}
