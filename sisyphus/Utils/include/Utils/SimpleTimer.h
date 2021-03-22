#pragma once
#include <cstdint>

/*
	Usage:
		auto timer = SimpleTimer::Start();
		* do work*
		double duration = timer.ElapsedSeconds();
*/

namespace Sisyphus {

	class SimpleTimer {
	public:
		static SimpleTimer Start();

		double ElapsedSeconds();

	private:
		SimpleTimer() = default;

		uint64_t start_us;
	};
}