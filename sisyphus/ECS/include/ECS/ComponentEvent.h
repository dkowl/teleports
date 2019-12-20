#pragma once
#include <concepts>
#include "uuid.h"

namespace Sisyphus::ECS {
	
	namespace ComponentEvents {
		class ComponentEventBase {};

		class Initialization : public ComponentEventBase {
		public:
			static uuids::uuid Id();
		};
	}

	// When adding a new event - update the following:
	//   - HandleEvent in IComponent (Component.h)
	//   - UpdateSubscriberLists<T> in Entity (Entity.h)

	template<typename T>
	concept ComponentEvent =
		std::derived_from<T, ComponentEvents::ComponentEventBase> &&
		requires {
			{T::Id()}->std::same_as<uuids::uuid>;
		};
}