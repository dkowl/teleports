#include "catch.hpp"
#define SIS_NO_DEBUG_BREAK
#include "Utils/FunctionFileLine.h"
#include "Utils/Throw.h"
#include "Utils/BreakAssert.h"
#include <string>
#include <iostream>

#define PRINT_AND_RETHROW( x ) try { x; } catch (std::exception& e) { std::cout << "Exception thrown! " << e.what() << std::endl; throw e; }

namespace {
	std::string Foo() {
		return SIS_FUNCTION_FILE_LINE;
	}

	void FooThrow() {
		PRINT_AND_RETHROW(SIS_THROW("bar"));
	}

	void FooThrowAssertNoMessage() {
		PRINT_AND_RETHROW(SIS_THROWASSERT(2 * 2 == 5, ));
	}

	void FooThrowAssert() {
		PRINT_AND_RETHROW(SIS_THROWASSERT(2 * 2 == 3, "Maths error"));
	}
}


TEST_CASE("Macros") {
	std::cout << "SIS_FUNCTION_FILE_LINE: " << Foo() << std::endl;
	REQUIRE_THROWS(FooThrow());
	REQUIRE_THROWS(FooThrowAssertNoMessage());
	REQUIRE_THROWS(FooThrowAssert());
}