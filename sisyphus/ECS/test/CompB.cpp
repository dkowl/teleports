#include "Pch_ECS_Test.h"
#include "CompB.h"
#include "CompA.h"
#include "CompC.h"

SIS_DEFINE_ID(ComponentID_CompB, "cab4ccc8f13e46a69930cc64382b869d");
SIS_REGISTER_COMPONENT(CompB);

bool CompB::compA_initialized = false;
bool CompB::compC_initialized = false;
bool CompB::customEventHandled = false;

CompB::~CompB()
{
	SIS_THROWASSERT(!Parent().HasComponent<CompC>());
}

uuids::uuid CompB::TypeId() {
	return ComponentID_CompB;
}

std::string CompB::ClassName() {
	return "CompB";
}

ComponentReferences CompB::Dependencies() {
	return { { CompA::TypeId() } };
}

void CompB::Initialize() {
}

void CompB::RegisterEventHandlers()
{
	RegisterEventHandler<Events::Initialization, CompA>([] {compA_initialized = true; });
	RegisterEventHandler<Events::Initialization, CompC>([] {compC_initialized = true; });
	RegisterEventHandler<CustomEvent, CompA>([] {customEventHandled = true; });
}
