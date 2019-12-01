#include "ComponentManager.h"
#include "Utils/Throw.h"
#include "Utils/Logger.h"

namespace Sisyphus::Rendering::Vulkan {
	void ComponentManager::InitComponent(ComponentType type)
	{
		SIS_THROWASSERT_MSG(!components.contains(type), ToString(type) + " already exists.");
		// TODO: Check if dependencies exist

		std::unique_ptr<Component> component = nullptr;
		switch (type) {
		case ComponentType::Instance:
			component = std::make_unique<Instance>();
			break;
		default:
			SIS_THROW("Unrecognized component type");
		}

		component->Initialize(this);
		components.emplace(type, std::move(component));
		
		Logger::Get().Log(ToString(type) + " initialized!");
	}

	const Component& ComponentManager::GetComponent(ComponentType type) const
	{
		auto comp = components.find(type);
		SIS_THROWASSERT(comp != components.end());
		return *comp->second;
	}
}