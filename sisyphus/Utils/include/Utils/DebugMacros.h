#pragma once

#ifdef _DEBUG
#define SIS_DEBUG
#endif

#ifdef SIS_DEBUG
#define SIS_DEBUG_ONLY(x) do { x; } while(0)
#else
#define SIS_DEBUG_ONLY(...) do {} while (0)
#endif