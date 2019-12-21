#include "Pch_ECS.h"
#include "ComponentEvent.h"
#include "Utils\UuidUtils.h"

namespace Sisyphus::ECS {
	namespace Events {
		SIS_DEFINE_ID(ComponentEventID_Initialization, "490cc459066042b6a9f82fad2c2467e7");

		uuids::uuid Initialization::Id()
		{
			return ComponentEventID_Initialization;
		}
	}
}