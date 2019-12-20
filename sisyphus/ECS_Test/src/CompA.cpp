#include "Pch_ECS_Test.h"
#include "CompA.h"
#include "CompB.h"
#include "CompC.h"

SIS_DEFINE_ID(ComponentID_CompA, "cab4ccc8f13e46a69930cc64382b869c");
SIS_REGISTER_COMPONENT(CompA);

bool CompA::compB_initialized = false;
bool CompA::compC_initialized = false;

uuids::uuid CompA::TypeId() {
	return ComponentID_CompA;
}

std::string CompA::ClassName() {
	return "CompA";
}

ComponentReferences CompA::Dependencies() {
	return {};
}

ComponentReferences CompA::WatchList(ComponentEvents::Initialization) {
	return { {CompB::TypeId()}, {CompC::TypeId() } };
}

void CompA::HandleEvent(ComponentEvents::Initialization, const uuids::uuid& type) {
	if (type == CompB::TypeId()) {
		compB_initialized = true;
	}
	else if (type == CompC::TypeId()) {
		compC_initialized = true;
	}
	else {
		SIS_THROW("Unexpected");
	}
}

void CompA::Initialize(const Entity&) {

}