#include "catch.hpp"
#include "Utils/PlatformMacros.h"
#include <fstream>
#include <iostream>
#include <cstdio>
#include <set>

#ifdef SIS_WINDOWS
#include <filesystem>
#endif

#ifdef SIS_ANDROID
#include "Globals.Android.h"
#endif
