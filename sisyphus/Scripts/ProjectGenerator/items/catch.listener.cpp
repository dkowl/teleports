#define CATCH_CONFIG_EXTERNAL_INTERFACES

#include "catch.hpp"
#include <string>
#include <sstream>
#include <memory>
#include "Logger/OstreamLogger.h"
#include "catch.globals.h"

std::string CatchGlobals::listenerOutput = "";

struct CatchListener : Catch::TestEventListenerBase {

	using TestEventListenerBase::TestEventListenerBase;

	void Log(const Catch::Counts& counts) {
		logger->Log(
			std::to_string(counts.passed) + "/" + std::to_string(counts.total()) 
			+ " passed - " + (counts.allPassed() ? "OK" : "FAIL"));
	}

	void testRunStarting(const Catch::TestRunInfo& testRunInfo) override {
		sstream.clear();
		logger = std::make_unique<Sisyphus::Logging::OstreamLogger>(sstream);

		logger->BeginSection("Test run:" + testRunInfo.name);
	}

	void testRunEnded(const Catch::TestRunStats& testRunStats) override {
		logger->EndSection();
		logger->LogInline("Test cases: ");
		Log(testRunStats.totals.testCases);
		logger->LogInline("Assertions: ");
		Log(testRunStats.totals.assertions);

		CatchGlobals::listenerOutput = sstream.str();
	}

	void testCaseStarting(const Catch::TestCaseInfo& testCaseInfo) override {
		logger->LogInline(testCaseInfo.name + " ");
	}

	void testCaseEnded(const Catch::TestCaseStats& testCaseStats) override {
		Log(testCaseStats.totals.assertions);
		if (!testCaseStats.stdErr.empty()) {
			logger->Log("ERROR: " + testCaseStats.stdErr);
		}
	}
private:
	std::stringstream sstream;
	std::unique_ptr<Sisyphus::Logging::OstreamLogger> logger;
};

CATCH_REGISTER_LISTENER(CatchListener)

