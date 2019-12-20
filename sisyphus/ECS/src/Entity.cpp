#include "Pch_ECS.h"
#include "ECS\Entity.h"
#include "Utils/DebugAssert.h"

namespace Sisyphus::ECS {

	IComponent& Entity::GetComponent(const uuids::uuid& type) const
	{
		auto comp = components.find(type);
		SIS_THROWASSERT(HasComponent(type));
		return *comp->second;
	}
	IComponent* Entity::TryGetComponent(const uuids::uuid& type) const
	{
		auto comp = components.find(type);
		return comp != components.end() ? comp->second.get() : nullptr;
	}
	Entity::~Entity()
	{
		DestroyAll();
	}
	void Entity::DestroyAll() {
		for (auto&& compType : dependencyGraph.GetDestructionOrder()) {
			SIS_DEBUGASSERT(HasComponent(compType));
			components[compType] = nullptr;
		}
		components.clear();
		subscriberLists.clear();
		dependencyGraph.Clear();
		knownComponentTypes.clear();
	}
	bool Entity::HasComponent(const uuids::uuid& type) const {
		auto findResult = components.find(type);
		return findResult != components.end() && findResult->second != nullptr;
	}
}