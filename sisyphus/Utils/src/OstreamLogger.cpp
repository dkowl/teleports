#include "Pch_Utils.h"
#include "OstreamLogger.h"
#include "DebugAssert.h"

namespace {
	constexpr char DecoratorChar = '=';
	constexpr int HeaderDecoratorWidth = 5;

	std::string HeaderDecorator() {
		return std::string(HeaderDecoratorWidth, DecoratorChar);
	}
}

OstreamLogger::OstreamLogger():
	output(&std::cout),
	currentLogLevel(0)
{

}

void OstreamLogger::Log(const std::string& message, int logLevel)
{
	if (logLevel > currentLogLevel) return;

	for (auto&& section : sections) {
		*output << section.indenter;
	}
	*output << message << "\n";
}

void OstreamLogger::BeginSection(std::string name, std::string indenter)
{
	Section section{ name, indenter };
	Log(section.Header());
	sections.emplace_back(section);
}

void OstreamLogger::EndSection()
{
	SIS_DEBUGASSERT(sections.size() > 0);
	if (sections.size() == 0) return;

	Section section = sections.back();
	sections.pop_back();
}

void OstreamLogger::SetLogLevel(int logLevel)
{
	currentLogLevel = logLevel;
}

std::string OstreamLogger::Section::Header() const
{
	return HeaderDecorator() + " " + name + " " + HeaderDecorator();
}

std::string OstreamLogger::Section::Footer() const
{
	int footerWidth = (HeaderDecoratorWidth + 1) * 2 + static_cast<int>(name.size());
	return std::string(footerWidth, DecoratorChar);
}
