#include "SimpleTimer.h"
#include "TimeUtils.h"
#include <chrono>

namespace Sisyphus {

	SimpleTimer SimpleTimer::Start() {
		SimpleTimer timer;
		timer.start_us = MicrosecondsSinceEpoch();
		return timer;
	}

	double SimpleTimer::ElapsedSeconds() {
		return double(MicrosecondsSinceEpoch() - start_us) / 1000000.0;
	}
}
