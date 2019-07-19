#pragma once

#ifdef _DEBUG
#include <string>
void BreakAssert(bool condition, std::string message = {});

template<typename T>
void BreakAssert(const T & condition, std::string message = {}) {
	BreakAssert(static_cast<bool>(condition), message);
}

#else
#define BreakAssert(...)
#endif
