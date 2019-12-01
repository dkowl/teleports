#include "BreakAssert.h"
#include <iostream>

#ifdef _DEBUG
void BreakAssert(bool condition, std::string message)
{
	if (!condition) {
		std::cerr << "Assertion failed! " << message << std::endl;
#if defined(_WIN32) && !defined(SIS_NO_DEBUG_BREAK)
		__debugbreak();
#endif
	}
}
#endif
