#pragma once
#include <vector>
#include "ComponentType.h"

namespace Sisyphus::Rendering::Vulkan {

	class ComponentManager;

	class Component {
	public:
		virtual ~Component() = default;

		void Initialize(ComponentManager* inManager);
		virtual ComponentType GetType() const = 0;
		virtual std::vector<ComponentType> GetDependencies() const = 0;

	protected:
		virtual void OnInitialize() = 0;

		ComponentManager* manager;
	};
}