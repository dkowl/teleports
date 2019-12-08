#pragma once
#include <vector>
#include <concepts>
#include "uuid.h"

namespace Sisyphus::Rendering::Vulkan {

#define SIS_DEFINE_COMPONENT_ID(componentName, id) \
	static const uuids::uuid ComponentID_##componentName = uuids::uuid::from_string(id).value(); 

	class ComponentManager;

	struct ComponentDependency {
		uuids::uuid componentType;
	};

	class IComponent {
	public:
		using Dependencies = std::vector<ComponentDependency>;

		virtual ~IComponent() = default;

		virtual void Initialize(const ComponentManager& manager) = 0;
	};

	template<typename VulkanType>
	class IVulkanComponent : public IComponent {
		virtual VulkanType GetVulkanObject() const = 0;
		virtual operator VulkanType() const = 0;
	};

	template <typename T>
	concept Component =
		std::derived_from<T, IComponent> &&
		requires { 
			{T::TypeId()}->std::same_as<uuids::uuid>;
			{T::ClassName()}->std::same_as<std::string>;
			{T::Dependencies()}->std::same_as<IComponent::Dependencies>;
		};

	template<typename T, typename VulkanType>
	concept VulkanComponent =
		Component<T> &&
		std::derived_from<T, IVulkanComponent<VulkanType>>;

}