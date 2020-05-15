#include "Section.h"

namespace Sisyphus::Logging {
	Section::Section(const std::string& inName, LogLevel inLogLevel, const std::string& inTag):
		name(inName),
		indenter("\t"),
		leftHeaderDecorator("====="),
		rightHeaderDecorator("====="),
		footer(""),
		logLevel(inLogLevel),
		tag(inTag)
	{}

	std::string Section::Header() const {
		return leftHeaderDecorator + " " + name + " " + rightHeaderDecorator;
	}

	std::string Section::Footer() const {
		return footer;
	}
}