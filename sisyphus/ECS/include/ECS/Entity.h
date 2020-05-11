#pragma once
#include "ECS\Component.h"
#include "ECS\DependencyGraph.h"
#include "Utils/Throw.h"
#include "Logger/Logger.h"
#include "Utils/StringUtils.h"
#include "uuid.h"
#include <vector>
#include <memory>
#include <unordered_map>

namespace Sisyphus::ECS {
	class Entity {
	public:

		Entity() = default;
		Entity(const Entity&) = delete;
		Entity(Entity&&) = delete;
		Entity& operator=(const Entity&) = delete;
		Entity& operator=(Entity&&) = delete;
		~Entity();

#ifdef __cpp_concepts
		template<Component T, typename... ConstructorArgs>
#else
		template<typename T, typename... ConstructorArgs>
#endif
		void InitComponent(ConstructorArgs... args) {
			uuids::uuid type = T::TypeId();
			SIS_THROWASSERT_MSG(components.find(type) == components.end(), T::ClassName() + " already exists.");
			CheckDependencies<T>();

			std::unique_ptr<IComponent> component = std::make_unique<T>(args...);
			component->entity = this;
			component->Initialize();
			component->RegisterEventHandlers();

			if (knownComponentTypes.find(type) == knownComponentTypes.end()) {
				UpdateSubscriberLists<T>(component->eventHandlers);
				dependencyGraph.Add<T>();
			}

			components.emplace(type, std::move(component));

			Dispatch<Events::Initialization, T>();
			Logger().Log(T::ClassName() + " initialized!");

			knownComponentTypes.insert(type);
		}

#ifdef __cpp_concepts
		template<Component T>
#else
		template<typename T>
#endif
		T& GetComponent() const {
			return static_cast<T&>(GetComponent(T::TypeId()));
		}

#ifdef __cpp_concepts
		template<Component T>
#else
		template<typename T>
#endif
		T* TryGetComponent() const {
			return static_cast<T*>(TryGetComponent(T::TypeId()));
		}

		void DestroyAll();

#ifdef __cpp_concepts
		template<Component T>
#else
		template<typename T>
#endif
		bool HasComponent() const {
			return HasComponent(T::TypeId());
		}

		bool HasComponent(const uuids::uuid& compType) const;

#ifdef __cpp_concepts
		template<ComponentEvent EventT, Component T>
#else
		template<typename EventT, typename T>
#endif
		void Dispatch() {
			for (auto subscriber : subscriberLists[EventT::Id()][T::TypeId()]) {
				auto comp = TryGetComponent(subscriber.type);
				if (comp != nullptr) {
					comp->HandleEvent(EventT::Id(), T::TypeId());
				}
			}
		}

	private:
#ifdef __cpp_concepts
		template<Component T>
#else
		template<typename T>
#endif
		void CheckDependencies() {
			for (auto&& dependency : T::Dependencies()) {
				if (!HasComponent(dependency.type)) {
					SIS_THROW(AssembleString(
						ComponentRegistry::GetComponentName(dependency.type),
						" must be initialized before ",
						T::ClassName())
					);
				}
			}
		}

#ifdef __cpp_concepts
		template<Component T>
#else
		template<typename T>
#endif
		void UpdateSubscriberLists(const IComponent::EventHandlerMap& eventHandlerMap) {
			for (auto&& perEventType : eventHandlerMap) {
				for (auto&& perCompType : perEventType.second) {
					subscriberLists[perEventType.first][perCompType.first].push_back({ T::TypeId() });
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
					ComponentReferences // Subscribers
				>
			>;
		SubscriberLists subscriberLists;
		DependencyGraph dependencyGraph;

		std::unordered_set<uuids::uuid> knownComponentTypes;
	};
}