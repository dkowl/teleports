#pragma once
#include <string>

namespace Sisyphus::Filesystem {

	class Path {
	public:
		Path();
		Path(const std::string& str);

		Path Stem();
		Path Filename();
		Path Dirname();
		Path Extension();

		std::string String();
	private:
		std::string pathString;
	};
}