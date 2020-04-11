#include "Pch_ECS_Test.h"
#include "CustomEvent.h"
#include "Utils/UuidUtils.h"

namespace {
	SIS_DEFINE_ID(ComponentEventID_CustomEvent, "c287ede671494af9b9d18d2828b3608b");
}

uuids::uuid CustomEvent::Id()
{
	return ComponentEventID_CustomEvent;
}
