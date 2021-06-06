#include "catch.hpp"
#include "Utils/DebugAssert.h"
#include "Utils/FunctionFileLine.h"
#include "Utils/Throw.h"
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

	void FooThrowAssert() {
		PRINT_AND_RETHROW(SIS_THROWASSERT(2 * 2 == 5));
	}

	void FooThrowAssertMsg() {
		PRINT_AND_RETHROW(SIS_THROWASSERT_MSG(2 * 2 == 3, "Maths error"));
	}
}


TEST_CASE("Macros") {
	std::cout << "SIS_FUNCTION_FILE_LINE: " << Foo() << std::endl;

	Sisyphus::Utils::DisableDebugBreak();
	REQUIRE_THROWS(FooThrow());
	REQUIRE_THROWS(FooThrowAssert());
	REQUIRE_THROWS(FooThrowAssertMsg());
	Sisyphus::Utils::EnableDebugBreak();

	std::cout << "SIS_DEBUGASSERT: ";
	SIS_DEBUGASSERT(1 == 2);

	std::cout << "SIS_DEBUGASSERT_MSG: ";
	SIS_DEBUGASSERT_MSG(2 == 3, "Maths error");
}