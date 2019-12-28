#include "Pch_VulkanRenderer.h"
#include "Events.h"
#include "Utils/UuidUtils.h"

namespace Sisyphus::Rendering::Vulkan {
	SIS_DEFINE_ID(ComponentEventID_Resize, "30621147ad3e4b069581c532075987c3");
	
	uuids::uuid ResizeEvent::Id()
	{
		return ComponentEventID_Resize;
	}

}