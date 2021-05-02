#pragma once

#ifdef _MSC_VER
#define SIS_MSVC
#endif

#ifdef __clang__
#define SIS_CLANG
#endif

#ifdef SIS_MSVC
#define SIS_DO_PRAGMA(x) __pragma(#x)

#define SIS_DISABLE_WARNINGS_BEGIN \
	SIS_DO_PRAGMA(warning(push, 0))

#define SIS_DISABLE_WARNING_END \
	SIS_DO_PRAGMA(warning(pop))

#elif defined(SIS_CLANG)
#define SIS_DO_PRAGMA(x) _Pragma(#x)

#define SIS_DISABLE_WARNINGS_BEGIN \
	SIS_DO_PRAGMA(clang diagnostic push) \
	SIS_DO_PRAGMA(clang diagnostic ignored "-Weverything")

#define SIS_DISABLE_WARNING_END \
	SIS_DO_PRAGMA(clang diagnostic pop)
#endif