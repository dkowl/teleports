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

		template<Component T, typename... ConstructorArgs>
		void InitComponent(ConstructorArgs... args) {
			uuids::uuid type = T::TypeId();
			SIS_THROWASSERT_MSG(!components.contains(type), T::ClassName() + " already exists.");

			if (!knownComponentTypes.contains(type)) {
				UpdateSubscriberLists<T>();
			}

			std::unique_ptr<IComponent> component = std::make_unique<T>(args...);
			component->Initialize(*this);
			components.emplace(type, std::move(component));

			Dispatch<T, ComponentEvents::Initialization>();
			Logger::Get().Log(T::ClassName() + " initialized!");

			knownComponentTypes.insert(type);
		}

		template<Component T>
		T& GetComponent() const {
			return dynamic_cast<T&>(GetComponent(T::TypeId()));
		}

	private:
		template<Component T>
		void UpdateSubscriberLists() {
			UpdateSubscriberLists<T, ComponentEvents::Initialization>();
		}

		template<Component T, ComponentEvent EventT>
		void UpdateSubscriberLists() {
			for (auto&& watched : T::WatchList(EventT{})) {
				subscriberLists[EventT::Id()][watched.componentType].push_back({ T::TypeId() });
			}
		}

		template<Component T, ComponentEvent EventT>
		void Dispatch() {
			for (auto subscriber : subscriberLists[EventT::Id()][T::TypeId()]) {
				auto comp = TryGetComponent(subscriber.componentType);
				if (comp != nullptr) {
					comp->HandleEvent(EventT{}, T::TypeId());
				}
			}
		}

		IComponent& GetComponent(const uuids::uuid& type) const;
		IComponent* TryGetComponent(const uuids::uuid& type) const;

		std::unordered_map<uuids::uuid, std::unique_ptr<IComponent>> components;

		using SubscriberLists = 
			std::unordered_map<
				uuids::uuid, // Event type
				std::unordered_map<
					uuids::uuid, // Component type
					IComponent::Dependencies // Subscribers
				>
			>;
		SubscriberLists subscriberLists;

		std::unordered_set<uuids::uuid> knownComponentTypes;
	};
}