#pragma once
#include <concepts>
#include "uuid.h"

namespace Sisyphus::Rendering::Vulkan {
	
	namespace ComponentEvents {
		class ComponentEventBase {};

		class Initialization : public ComponentEventBase {
		public:
			static uuids::uuid Id();
		};
	}

	// When adding a new event - update the following:
	//   - ComponentEvent concept (this file)
	//   - HandleEvent and WatchList method in IComponent (Component.h)
	//   - UpdateSubscriberLists<T> in ComponentManager (ComponentManager.h)

	template<typename T>
	concept ComponentEvent =
		std::derived_from<T, ComponentEvents::ComponentEventBase> &&
		requires {
			{T::Id()}->std::same_as<uuids::uuid>;
		};
}