#include "Pch_VulkanRenderer.h"
#include "Surface.h"
#include "ComponentManager.h"
#include "Instance.h"
#include "VulkanUtils.h"
#include "WindowCreator\WindowCreator.h"
#include "Utils\Throw.h"
#include "Utils/DebugAssert.h"

namespace Sisyphus::Rendering::Vulkan {
	SIS_DEFINE_ID(ComponentID_Surface, "a070642c63c04385818404553f6a97d3");

	Surface::Surface(WindowCreator::Window* inWindow):
		window(inWindow),
		surface(nullptr)
	{
		SIS_THROWASSERT(window);
	}
	void Surface::Initialize(const ComponentManager& componentManager)
	{
		surface = window->GetVulkanSurface(componentManager.GetComponent<Instance>());
		SIS_THROWASSERT(*surface);
	}
	uuids::uuid Surface::TypeId()
	{
		return ComponentID_Surface;
	}
	std::string Surface::ClassName()
	{
		return "Surface";
	}
	IComponent::Dependencies Surface::Dependencies()
	{
		return { {Instance::TypeId()} };
	}
	vk::SurfaceKHR Surface::GetVulkanObject() const
	{
		return *surface;
	}
	vk::Extent2D Surface::GetExtent() const
	{
		SIS_DEBUGASSERT(window);
		return GetExtent2D(window->GetExtent());
	}
}