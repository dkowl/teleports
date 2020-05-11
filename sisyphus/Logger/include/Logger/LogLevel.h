#pragma once

namespace Sisyphus {
	enum class LogLevel : int {
		None = 0,
		Fatal = 100,
		Error = 200,
		Warn = 300,
		Info = 400,
		Debug = 500,
		Trace = 600,
		All = INT_MAX
	};
}