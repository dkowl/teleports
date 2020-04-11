#pragma once
#include <string>

namespace Sisyphus::Logging {
	struct Section {
		Section(const std::string& name);

		std::string name;
		std::string indenter;
		std::string leftHeaderDecorator;
		std::string rightHeaderDecorator;
		std::string footer;

		std::string Header() const;
		std::string Footer() const;
	};
}