#pragma once
#ifdef _WIN32
#define VK_USE_PLATFORM_WIN32_KHR
#undef WIN_32_LEAN_AND_MEAN
#define WIN32_LEAN_AND_MEAN
#undef NOMINMAX
#define NOMINMAX
#include <vulkan/vulkan.hpp>
#endif