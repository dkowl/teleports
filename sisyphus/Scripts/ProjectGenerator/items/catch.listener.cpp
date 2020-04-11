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
		Sisyphus::Logging::Section section(testCaseInfo.name);
		section.leftHeaderDecorator = "(";
		section.rightHeaderDecorator = ")";
		logger->BeginSection(section);
	}

	void testCaseEnded(const Catch::TestCaseStats& testCaseStats) override {
		Log(testCaseStats.totals.assertions);
		if (!testCaseStats.stdErr.empty()) {
			logger->Log("ERROR: " + testCaseStats.stdErr);
		}
		logger->EndSection();
	}

	bool assertionEnded(const Catch::AssertionStats& assertionStats) override {
		const Catch::AssertionResult& result = assertionStats.assertionResult;
		if (!result.isOk()) {
			logger->BeginSection("Assertion FAILED");
			logger->Log(std::string(result.getSourceInfo().file) + ": line " + std::to_string(result.getSourceInfo().line));
			logger->Log(result.getExpressionInMacro() + " expanded to " + result.getExpandedExpression());
			if (result.hasMessage()) {
				logger->Log("Message: " + result.getMessage());
			}
			logger->EndSection();
		}
		return true;
	}
private:
	std::stringstream sstream;
	std::unique_ptr<Sisyphus::Logging::OstreamLogger> logger;
};

CATCH_REGISTER_LISTENER(CatchListener)

