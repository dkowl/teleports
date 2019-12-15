#pragma once
#include <string>
#include <sstream>

namespace Sisyphus {
	using String = ::std::string;

	template<typename... Args>
	String AssembleString(Args... args) {
		std::stringstream ss;
		(ss << ... << args);
		return ss.str();
	}
}