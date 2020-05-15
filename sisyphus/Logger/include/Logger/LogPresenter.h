#pragma once

#include "LogLevel.h"
#include <string>

namespace Sisyphus::Logging {

	class LogPresenter {
	public:
		virtual ~LogPresenter() = default;

		virtual void Present(const std::string& s, LogLevel logLevel, const std::string& tag = "") = 0;
	};
}