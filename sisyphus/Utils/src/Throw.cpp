#include "Pch_Utils.h"
#include "Throw.h"

namespace Sisyphus::Utils {
	void ThrowAssert(bool condition, const String& message)
	{
		if (!condition) {
			Throw(message);
		}
	}
}
