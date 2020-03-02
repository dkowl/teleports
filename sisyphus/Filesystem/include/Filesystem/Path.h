#pragma once
#include <string>

namespace Sisyphus::Fs {

	class Path {
	public:
		Path();
		Path(const std::string& str);

		Path Stem() const;
		Path Filename() const;
		Path Dirname() const;
		Path Extension() const;

		std::string String() const;
	private:
		std::string pathString;
	};
}