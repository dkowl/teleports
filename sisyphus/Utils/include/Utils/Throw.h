#pragma once
#include <stdexcept>
#include "Utils\StringUtils.h"
#include "FunctionFileLine.h"

#define SIS_THROW(message) do { Sisyphus::Utils::Throw(std::string(message) + " at " + SIS_FUNCTION_FILE_LINE); } while(0)
#define SIS_THROWASSERT(condition, message) do { Sisyphus::Utils::ThrowAssert(condition, std::string(message) + " " #condition " == false at " + SIS_FUNCTION_FILE_LINE); } while (0)

namespace Sisyphus::Utils {
	inline void Throw(const String& message)
	{
#if defined(_DEBUG) && defined(_WIN32) && !defined(SIS_NO_DEBUG_BREAK)
		__debugbreak();
#endif
		throw std::runtime_error(message);
	}
	void ThrowAssert(bool condition, const String& message);
}