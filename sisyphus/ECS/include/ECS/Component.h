#pragma once
#include <vector>
#include <concepts>
#include "uuid.h"
#include "ECS\ComponentEvent.h"
#include "ECS\ComponentRegistry.h"
#include "Utils\UuidUtils.h"

namespace Sisyphus::ECS {

#define SIS_REGISTER_COMPONENT(type) \
	static bool s_##type##_registered = ComponentRegistry::Register(type##::TypeId(), type##::ClassName())


	class Entity;

	struct ComponentReference {
		uuids::uuid type;
	};
	using ComponentReferences = std::vector<ComponentReference>;

	class IComponent;

	template <typename T>
	concept Component =
		std::derived_from<T, IComponent> &&
		requires {
			{T::TypeId()}->std::same_as<uuids::uuid>;
			{T::ClassName()}->std::same_as<std::string>;
			{T::Dependencies()}->std::same_as<ComponentReferences>;
	};

	class IComponent {
	public:
		IComponent() = default;
		virtual ~IComponent() = default;
		IComponent(const IComponent&) = delete;
		IComponent(IComponent&&) = delete;
		IComponent& operator=(const IComponent&) = delete;
		IComponent& operator=(IComponent&&) = delete;

		friend class Entity;
		Entity& Parent();

		virtual void Initialize() = 0;

		using EventHandler = std::function<void()>;
		template<ComponentEvent EventT, Component T>
		void RegisterEventHandler(EventHandler handler) {
			eventHandlers[EventT::Id()][T::TypeId()] = std::move(handler);
		}
		virtual void RegisterEventHandlers() {};
		
		void HandleEvent(const uuids::uuid& eventId, const uuids::uuid& compId);

	private:
		using EventHandlerMap = std::unordered_map<uuids::uuid, std::unordered_map<uuids::uuid, EventHandler>>;
		EventHandlerMap eventHandlers;
		Entity* entity;
	};
}