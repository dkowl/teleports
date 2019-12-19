#pragma once
#include <vector>
#include <concepts>
#include "uuid.h"
#include "ComponentEvent.h"

namespace Sisyphus::Rendering::Vulkan {

#define SIS_DEFINE_ID(name, id) \
	static const uuids::uuid name = uuids::uuid::from_string(id).value(); 

	class ComponentManager;

	struct ComponentReference {
		uuids::uuid type;
	};
	using ComponentReferences = std::vector<ComponentReference>;

	class IComponent {
	public:
		IComponent() = default;
		virtual ~IComponent() = default;
		IComponent(const IComponent&) = delete;
		IComponent(IComponent&&) = delete;
		IComponent& operator=(const IComponent&) = delete;
		IComponent& operator=(IComponent&&) = delete;


		virtual void Initialize(const ComponentManager& manager) = 0;

		virtual void HandleEvent(ComponentEvents::Initialization, const uuids::uuid& /*compTypeId*/) {};

		static ComponentReferences WatchList(ComponentEvents::Initialization) { return ComponentReferences(); }
	};

	template<typename VulkanType>
	class IVulkanComponent : public IComponent {
	public:
		virtual VulkanType GetVulkanObject() const = 0;

		operator VulkanType() const {
			return GetVulkanObject();
		}
	};

	template <typename T>
	concept Component =
		std::derived_from<T, IComponent> &&
		requires { 
			{T::TypeId()}->std::same_as<uuids::uuid>;
			{T::ClassName()}->std::same_as<std::string>;
			{T::Dependencies()}->std::same_as<ComponentReferences>;
			// {T::WatchList(ComponentEvent::Initialization{})}->std::same_as<ComponentReferences>;
		};

	template<typename T, typename VulkanType>
	concept VulkanComponent =
		Component<T> &&
		std::derived_from<T, IVulkanComponent<VulkanType>>;

}