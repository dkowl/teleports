#pragma once
#include "Utils/DebugMacros.h"

#ifdef SIS_DEBUG
#include <string>
#include <iostream>
#include "Utils/FunctionFileLine.h"

#define SIS_DEBUGASSERT(condition) \
	do { Sisyphus::Utils::DebugAssert( \
		condition, \
		std::string("Condition \"" #condition "\" is false at ") + SIS_FUNCTION_FILE_LINE + "."); \
	} while (0)

#define SIS_DEBUGASSERT_MSG(condition, message) \
	do { Sisyphus::Utils::DebugAssert( \
		condition, \
		std::string("Condition \"" #condition "\" is false at ") + SIS_FUNCTION_FILE_LINE + \
		". Message: " + std::string(message)); \
	} while (0)

namespace Sisyphus::Utils {
	inline void DebugAssert(bool condition, std::string message)
	{
		if (!condition) {
			std::cerr << "Assertion failed! " << message << std::endl;
#if defined(_WIN32) && !defined(SIS_NO_DEBUG_BREAK)
			__debugbreak();
#endif
		}
	}

	template<typename T>
	void DebugAssert(const T& condition, std::string message = {}) {
		DebugAssert(static_cast<bool>(condition), message);
	}
}

#else
#define SIS_DEBUGASSERT(...)
#define SIS_DEBUGASSERT_MSG(...)
#endif