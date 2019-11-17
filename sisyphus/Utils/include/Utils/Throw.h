#pragma once
#include "Utils\StringUtils.h"

namespace Sisyphus::Utils {
	void Throw(const String& message);
	void ThrowAssert(bool condition, const String& message = String{});
}