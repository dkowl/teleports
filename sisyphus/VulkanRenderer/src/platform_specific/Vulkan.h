#pragma once
#include "Utils/Platform.h"
#ifdef SIS_WINDOWS
#define VK_USE_PLATFORM_WIN32_KHR
#undef WIN_32_LEAN_AND_MEAN
#define WIN32_LEAN_AND_MEAN
#undef NOMINMAX
#define NOMINMAX
#elif defined(SIS_ANDROID)
#define VK_USE_PLATFORM_ANDROID_KHR
#endif
#include <vulkan/vulkan.hpp>