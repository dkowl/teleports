#include "catch.hpp"
#include "Utils/Platform.h"
#include <fstream>
#include <iostream>
#include <cstdio>
#include <set>

#ifdef SIS_WINDOWS
#include <filesystem>
#endif

#ifdef SIS_ANDROID
#include "AndroidGlobals/Globals.Android.h"
#endif
