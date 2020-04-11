#pragma once
#include "ECS/Component.h"

namespace Sisyphus::Rendering::Vulkan {

	template<typename VulkanType>
	class IVulkanComponent : public ECS::IComponent {
	public:
		virtual VulkanType GetVulkanObject() const = 0;

		operator VulkanType() const {
			return GetVulkanObject();
		}
	};

#ifdef __cpp_concepts
	template<typename T, typename VulkanType>
	concept VulkanComponent =
		ECS::Component<T> &&
		std::derived_from<T, IVulkanComponent<VulkanType>>;
#endif
}