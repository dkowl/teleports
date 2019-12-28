#include "Pch_ECS_Test.h"
#include "CompB.h"
#include "CompA.h"
#include "CompC.h"

SIS_DEFINE_ID(ComponentID_CompB, "cab4ccc8f13e46a69930cc64382b869d");
SIS_REGISTER_COMPONENT(CompB);

bool CompB::compA_initialized = false;
bool CompB::compC_initialized = false;

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

ComponentReferences CompB::WatchList(Events::Initialization) {
	return { {CompA::TypeId()}, {CompC::TypeId() } };
}

void CompB::HandleEvent(Events::Initialization, const uuids::uuid& type) {
	if (type == CompA::TypeId()) {
		compA_initialized = true;
	}
	else if (type == CompC::TypeId()) {
		compC_initialized = true;
	}
	else {
		SIS_THROW("Unexpected");
	}
}

void CompB::Initialize() {
}