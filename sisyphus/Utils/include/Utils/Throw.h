#pragma once
#include <stdexcept>
#include "Utils\DebugMacros.h"
#include "Utils\StringUtils.h"
#include "Utils\FunctionFileLine.h"

#define SIS_THROW(message) \
	do { Sisyphus::Utils::Throw( \
		std::string(message) + " at " + SIS_FUNCTION_FILE_LINE + "."); \
	} while(0)

#define SIS_THROWASSERT(condition) \
	do { Sisyphus::Utils::ThrowAssert( \
		condition, \
		std::string("Condition \"" #condition "\" is false at ") + SIS_FUNCTION_FILE_LINE + "."); \
	} while (0)

#define SIS_THROWASSERT_MSG(condition, message) \
	do { Sisyphus::Utils::ThrowAssert( \
		condition, \
		std::string("Condition \"" #condition "\" is false at ") + SIS_FUNCTION_FILE_LINE + \
		". Message: " + std::string(message)); \
	} while (0)

namespace Sisyphus::Utils {
	inline void Throw(const String& message)
	{
#if !defined(SIS_NO_DEBUG_BREAK)
		SIS_DEBUG_BREAK;
#endif
		throw std::runtime_error(message);
	}
	void ThrowAssert(bool condition, const String& message);

	template<typename T>
	void ThrowAssert(T condition, const String& message) {
		ThrowAssert(static_cast<bool>(condition), message);
	}
}