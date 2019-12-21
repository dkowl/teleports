#include "Pch_ECS.h"
#include "ComponentEvent.h"

namespace Sisyphus::ECS {
	namespace Events {
		uuids::uuid Initialization::Id()
		{
			return uuids::uuid();
		}
	}
}