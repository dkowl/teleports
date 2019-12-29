#pragma once
#include "ECS/ComponentEvent.h"
#include "uuid.h"

using namespace Sisyphus::ECS::Events;

class CustomEvent : public ComponentEventBase {
public:
	static uuids::uuid Id();
};