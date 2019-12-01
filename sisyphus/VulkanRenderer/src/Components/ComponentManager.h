#pragma once
#include "Component.h"
#include "Instance.h"
#include <vector>
#include <memory>
#include <unordered_map>

#define SIS_REN_VUL_DEFINE_GET_COMPONENT(type) \
	template<> \
	const type& GetComponent() const { \
		return dynamic_cast<const type&>(GetComponent(ComponentType::type)); \
	}

namespace Sisyphus::Rendering::Vulkan {
	class ComponentManager {
	public:
		void InitComponent(ComponentType type);

		template<typename T>
		const T& GetComponent() const {
			static_assert(false, "Refer to a specific specialization");
		}
		SIS_REN_VUL_DEFINE_GET_COMPONENT(Instance);

	private:
		const Component& GetComponent(ComponentType type) const;
		std::unordered_map<ComponentType, std::unique_ptr<Component>> components;
	};
}