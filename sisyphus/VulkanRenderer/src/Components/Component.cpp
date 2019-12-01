#include "Component.h"

namespace Sisyphus::Rendering::Vulkan {
	void Component::Initialize(ComponentManager* inManager)
	{
		this->manager = inManager;
		OnInitialize();
	}
}