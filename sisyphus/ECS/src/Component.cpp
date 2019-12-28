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
}
