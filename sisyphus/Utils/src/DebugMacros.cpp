#include "DebugMacros.h"

namespace Sisyphus::Utils {
	thread_local bool debugBreakEnabled = true;

	bool DebugBreakEnabled() {
		return debugBreakEnabled;
	}

	void EnableDebugBreak() {
		debugBreakEnabled = true;
	}

	void DisableDebugBreak() {
		debugBreakEnabled = false;
	}
}