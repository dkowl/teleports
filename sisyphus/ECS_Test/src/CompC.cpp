#include "Pch_ECS_Test.h"
#include "CompC.h"
#include "CompB.h"

SIS_DEFINE_ID(ComponentID_CompC, "cab4ccc8f13e46a69930cc64382b869e");
SIS_REGISTER_COMPONENT(CompC);

uuids::uuid CompC::TypeId() {
	return ComponentID_CompC;
}

std::string CompC::ClassName() {
	return "CompC";
}

ComponentReferences CompC::Dependencies() {
	return { {CompB::TypeId()} };
}

void CompC::Initialize(const Entity& inEntity) {
	initialized = true;
	entity = &inEntity;
}