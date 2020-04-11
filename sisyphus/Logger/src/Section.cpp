#include "Section.h"

namespace Sisyphus::Logging {
	Section::Section(const std::string& name):
		name(name),
		indenter("\t"),
		leftHeaderDecorator("====="),
		rightHeaderDecorator("====="),
		footer("")
	{}

	std::string Section::Header() const {
		return leftHeaderDecorator + " " + name + " " + rightHeaderDecorator;
	}

	std::string Section::Footer() const {
		return footer;
	}
}