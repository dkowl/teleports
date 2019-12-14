#include "Pch_VulkanRenderer.h"
#include "ComponentManager.h"

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
}