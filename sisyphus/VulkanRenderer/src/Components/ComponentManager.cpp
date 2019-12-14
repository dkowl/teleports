#include "Pch_VulkanRenderer.h"
#include "ComponentManager.h"

namespace Sisyphus::Rendering::Vulkan {

	const IComponent& ComponentManager::GetComponent(uuids::uuid type) const
	{
		auto comp = components.find(type);
		SIS_THROWASSERT(comp != components.end());
		return *comp->second;
	}
}