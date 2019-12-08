#pragma once
#include "Component.h"
#include "Utils/Throw.h"
#include "Utils/Logger.h"
#include "uuid.h"
#include <vector>
#include <memory>
#include <unordered_map>

namespace Sisyphus::Rendering::Vulkan {
	class ComponentManager {
	public:

		template<Component T>
		void InitComponent() {
			uuids::uuid type = T::TypeId();
			SIS_THROWASSERT_MSG(!components.contains(type), T::ClassName() + " already exists.");

			std::unique_ptr<IComponent> component = std::make_unique<T>();
			component->Initialize(*this);
			components.emplace(type, std::move(component));

			Logger::Get().Log(T::ClassName() + " initialized!");
		}

		template<Component T>
		const T& GetComponent() const {
			return dynamic_cast<const T&>(GetComponent(T::TypeId()));
		}

	private:
		const IComponent& GetComponent(uuids::uuid type) const;
		std::unordered_map<uuids::uuid, std::unique_ptr<IComponent>> components;
	};
}