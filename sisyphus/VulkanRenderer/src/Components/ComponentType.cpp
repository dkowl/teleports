#include "ComponentType.h"
#include "Utils/Throw.h"

namespace Sisyphus::Rendering::Vulkan {
	std::string ToString(ComponentType type)
	{
		switch (type) {
		case ComponentType::Instance:
			return "Instance";
		default:
			Utils::Throw("Component type not recognized");
			return "";
		}
	}
}