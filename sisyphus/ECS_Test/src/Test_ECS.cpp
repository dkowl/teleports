#include "Pch_ECS_Test.h"
#include "ECS/Entity.h"
#include "CompA.h"
#include "CompB.h"
#include "CompC.h"

using namespace Sisyphus::ECS;

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
	REQUIRE(e.GetComponent<CompA>().compB_initialized);
	REQUIRE(!e.GetComponent<CompA>().compC_initialized);
	REQUIRE(!e.GetComponent<CompB>().compA_initialized);

	e.InitComponent<CompC>();
	REQUIRE(e.HasComponent<CompC>());
	REQUIRE(e.GetComponent<CompA>().compC_initialized);
	REQUIRE(e.GetComponent<CompB>().compC_initialized);
	REQUIRE(e.GetComponent<CompC>().initialized);

	REQUIRE_NOTHROW(e.DestroyAll());

	REQUIRE_THROWS(e.GetComponent<CompA>());
	REQUIRE(e.TryGetComponent<CompB>() == nullptr);

	REQUIRE(ComponentRegistry::GetComponentName(CompA::TypeId()) == "CompA");
	REQUIRE(ComponentRegistry::GetComponentName(CompC::TypeId()) == "CompC");
}