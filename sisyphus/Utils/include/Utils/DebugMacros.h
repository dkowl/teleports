#pragma once
#include "Utils/CompilerMacros.h"

#if defined(_DEBUG) || !defined(NDEBUG)
#define SIS_DEBUG
#endif

#ifdef SIS_DEBUG
#define SIS_DEBUG_ONLY(x) do { x; } while(0)
#else
#define SIS_DEBUG_ONLY(...) do {} while (0)
#endif

#ifdef SIS_DEBUG
#ifdef _WIN32
#define SIS_DEBUG_BREAK __debugbreak()
#else
#define SIS_DEBUG_BREAK abort()
#endif
#else
#define SIS_DEBUG_BREAK void(0)
#endif