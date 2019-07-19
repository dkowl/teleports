#include "BreakAssert.h"
#include <iostream>

#ifdef _DEBUG
void BreakAssert(bool condition, std::string message)
{
	if (!condition) {
		std::cerr << message << std::endl;
#ifdef _WIN32
		__debugbreak();
#endif
	}
}
#endif
