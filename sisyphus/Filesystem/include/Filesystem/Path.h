#pragma once
#include <string>

namespace Sisyphus::Fs {

	class Path {
	public:
		Path();
		Path(const char* cStr);
		Path(const std::string& str);

		Path& operator/=(const Path& p);
		template<typename T>
		Path& operator/=(const T& p) {
			return *this /= Path(p);
		}

		friend Path operator/(Path p1, const Path& p2) {
			p1 /= p2;
			return p1;
		}

		Path Stem() const;
		Path Filename() const;
		Path Dirname() const;
		Path Extension() const;
		std::string LastSegment() const;
		bool Empty() const;

		const char* CStr() const;
		const std::string& String() const;

		static bool IsSeparator(const char* s);
	private:
		std::string pathString;
	};
}