#include "Pch_ECS.h"
#include "Component.h"
#include "Entity.h"
#include "Utils/DebugAssert.h"

namespace Sisyphus::ECS {

	Entity& IComponent::Parent()
	{
		SIS_DEBUGASSERT(entity);
		return *entity;
	}
	void IComponent::HandleEvent(const uuids::uuid& eventId, const uuids::uuid& compId)
	{
		eventHandlers.at(eventId).at(compId)();
	}
}
