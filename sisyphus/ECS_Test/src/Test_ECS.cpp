#include "catch.hpp"
#define SIS_NO_DEBUG_BREAK
#include "uuid.h"
#include "ECS/Entity.h"

using namespace Sisyphus::ECS;

SIS_DEFINE_ID(ComponentID_CompA, "cab4ccc8f13e46a69930cc64382b869c");
SIS_DEFINE_ID(ComponentID_CompB, "cab4ccc8f13e46a69930cc64382b869d");
SIS_DEFINE_ID(ComponentID_CompC, "cab4ccc8f13e46a69930cc64382b869e");

class CompA : public IComponent {
public:
	static uuids::uuid TypeId() {
		return ComponentID_CompA;
	}

	static std::string ClassName() {
		return "CompA";
	}

	static ComponentReferences Dependencies() {
		return {};
	}

	void Initialize(const Entity&) override {

	}
};
SIS_REGISTER_COMPONENT(CompA);

class CompB : public IComponent {
public:
	static uuids::uuid TypeId() {
		return ComponentID_CompB;
	}

	static std::string ClassName() {
		return "CompB";
	}

	static ComponentReferences Dependencies() {
		return { {CompA::TypeId()} };
	}

	void Initialize(const Entity&) override {

	}
};
SIS_REGISTER_COMPONENT(CompB);

TEST_CASE("Entity") {
	Entity e;
	REQUIRE(!e.HasComponent<CompA>());

	REQUIRE_THROWS(e.InitComponent<CompB>());
	REQUIRE(!e.HasComponent<CompB>());

	e.InitComponent<CompA>();
	REQUIRE(e.HasComponent<CompA>());
	REQUIRE(!e.HasComponent<CompB>());


}