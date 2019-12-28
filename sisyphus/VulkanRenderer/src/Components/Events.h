#pragma once
#include "ECS/ComponentEvent.h"
#include "uuid.h"

namespace Sisyphus::Rendering::Vulkan{
	class ResizeEvent : public ECS::Events::ComponentEventBase {
	public:
		static uuids::uuid Id();
	};
}