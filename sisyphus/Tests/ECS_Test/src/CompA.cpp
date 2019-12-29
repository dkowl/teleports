#include "Pch_ECS_Test.h"
#include "CompA.h"
#include "CompB.h"
#include "CompC.h"
#include "ECS/Entity.h"

SIS_DEFINE_ID(ComponentID_CompA, "cab4ccc8f13e46a69930cc64382b869c");
SIS_REGISTER_COMPONENT(CompA);

bool CompA::compB_initialized = false;
bool CompA::compC_initialized = false;

CompA::~CompA()
{
	SIS_THROWASSERT(!(Parent().HasComponent<CompB>() || Parent().HasComponent<CompC>()));
}

uuids::uuid CompA::TypeId() {
	return ComponentID_CompA;
}

std::string CompA::ClassName() {
	return "CompA";
}

ComponentReferences CompA::Dependencies() {
	return {};
}

void CompA::Initialize() {
}

void CompA::RegisterEventHandlers()
{
	RegisterEventHandler<Events::Initialization, CompB>([] {compB_initialized = true; });
	RegisterEventHandler<Events::Initialization, CompC>([] {compC_initialized = true; });
}

void CompA::DispatchCustomEvent()
{
	Parent().Dispatch<CustomEvent, CompA>();
}
