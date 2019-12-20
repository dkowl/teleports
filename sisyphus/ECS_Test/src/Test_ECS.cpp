#include "catch.hpp"
#define SIS_NO_DEBUG_BREAK
#include "uuid.h"
#include "ECS/Entity.h"
#include "CompA.h"
#include "CompB.h"
#include "CompC.h"

TEST_CASE("Entity") {
	Entity e;
	REQUIRE(!e.HasComponent<CompA>());

	REQUIRE_THROWS(e.InitComponent<CompB>());
	REQUIRE(!e.HasComponent<CompB>());

	e.InitComponent<CompA>();
	REQUIRE(e.HasComponent<CompA>());
	REQUIRE(!e.HasComponent<CompB>());

	e.InitComponent<CompB>();
	REQUIRE(e.HasComponent<CompB>());

	e.InitComponent<CompC>();


}