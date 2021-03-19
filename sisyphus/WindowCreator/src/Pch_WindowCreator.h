#pragma once
#include "Utils/Platform.h"
#ifdef SIS_WINDOWS
#define VK_USE_PLATFORM_WIN32_KHR
#elif defined(SIS_ANDROID)
#define VK_USE_PLATFORM_ANDROID_KHR
#endif
#include <vulkan\vulkan.hpp>
#include <optional>
#include <memory>