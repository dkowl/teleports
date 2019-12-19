#include "Pch_VulkanRenderer.h"
#include "ComponentManager.h"
#include "Utils/DebugAssert.h"

namespace Sisyphus::Rendering::Vulkan {

	IComponent& ComponentManager::GetComponent(const uuids::uuid& type) const
	{
		auto comp = components.find(type);
		SIS_THROWASSERT(comp != components.end());
		return *comp->second;
	}
	IComponent* ComponentManager::TryGetComponent(const uuids::uuid& type) const
	{
		auto comp = components.find(type);
		return comp != components.end() ? comp->second.get() : nullptr;
	}
	ComponentManager::~ComponentManager()
	{
		DestroyAll();
	}
	void ComponentManager::DestroyAll() {
		for (auto&& compType : dependencyGraph.GetDestructionOrder()) {
			SIS_DEBUGASSERT(HasComponent(compType));
			components[compType] = nullptr;
		}
	}
	bool ComponentManager::HasComponent(const uuids::uuid& type) {
		auto findResult = components.find(type);
		return findResult != components.end() && findResult->second != nullptr;
	}
}