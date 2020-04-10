#include "BasicLogger.h"
#include "Utils/DebugAssert.h"

namespace {
	constexpr char DecoratorChar = '=';
	constexpr int HeaderDecoratorWidth = 5;

	std::string HeaderDecorator() {
		return std::string(HeaderDecoratorWidth, DecoratorChar);
	}
}

namespace Sisyphus::Logging {
	BasicLogger::BasicLogger() :
		currentLogLevel(0)
	{

	}

	void BasicLogger::Log(const std::string& message, int logLevel)
	{
		if (logLevel > currentLogLevel) return;

		for (auto&& section : sections) {
			Output(section.indenter);
		}
		Output(message);
		Output("\n");
	}

	void BasicLogger::BeginSection(std::string name, std::string indenter)
	{
		Section section{ name, indenter };
		Log(section.Header());
		sections.emplace_back(section);
	}

	void BasicLogger::EndSection()
	{
		SIS_DEBUGASSERT(sections.size() > 0);
		if (sections.size() == 0) return;

		Section section = sections.back();
		sections.pop_back();
	}

	void BasicLogger::SetLogLevel(int logLevel)
	{
		currentLogLevel = logLevel;
	}

	std::string BasicLogger::Section::Header() const
	{
		return HeaderDecorator() + " " + name + " " + HeaderDecorator();
	}

	std::string BasicLogger::Section::Footer() const
	{
		int footerWidth = (HeaderDecoratorWidth + 1) * 2 + static_cast<int>(name.size());
		return std::string(footerWidth, DecoratorChar);
	}
}
