#include "Pch_VulkanRenderer.h"
#include "Surface.h"
#include "ECS/Entity.h"
#include "Instance.h"
#include "PhysicalDevice.h"
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
	Surface::~Surface()
	{
		SIS_DEBUG_ONLY(Logger::Get().Log("~Surface"));
	}
	void Surface::Initialize()
	{
		surface = window->GetVulkanSurface(Parent().GetComponent<Instance>());
		SIS_THROWASSERT(*surface);
		InitFormatAndColorSpace();
	}
	uuids::uuid Surface::TypeId()
	{
		return ComponentID_Surface;
	}
	std::string Surface::ClassName()
	{
		return "Surface";
	}
	ECS::ComponentReferences Surface::Dependencies()
	{
		return { {Instance::TypeId()}, {PhysicalDevice::TypeId()} };
	}
	vk::SurfaceKHR Surface::GetVulkanObject() const
	{
		return *surface;
	}
	void Surface::InitFormatAndColorSpace()
	{
		auto& logger = Logger::Get();

		constexpr vk::Format desiredFormat = vk::Format::eB8G8R8A8Srgb;
		constexpr vk::ColorSpaceKHR desiredColorSpace = vk::ColorSpaceKHR::eSrgbNonlinear;
		bool formatFound = false;

		auto surfaceFormats = Parent().GetComponent<PhysicalDevice>().GetVulkanObject().getSurfaceFormatsKHR(*surface);
		logger.BeginSection("Surface formats:");
		for (int i = 0; i < surfaceFormats.size(); i++) {
			const auto& surfaceFormat = surfaceFormats[i];
			logger.BeginSection("#" + std::to_string(i) + ":");
			logger.Log("Format: " + vk::to_string(surfaceFormat.format));
			logger.Log("Color Space: " + vk::to_string(surfaceFormat.colorSpace));
			logger.EndSection();

			if (surfaceFormat.format == desiredFormat && surfaceFormat.colorSpace == desiredColorSpace) {
				formatFound = true;
				break;
			}
		}
		logger.EndSection();
		if (!formatFound) {
			std::stringstream ss;
			ss << "Unable to find desired format: " << vk::to_string(desiredFormat)
				<< " and color space: " << vk::to_string(desiredColorSpace);
			SIS_THROW(ss.str());
		}
		format = desiredFormat;
		colorSpace = desiredColorSpace;
	}
	vk::Extent2D Surface::GetExtent() const
	{
		SIS_DEBUGASSERT(window);
		return GetExtent2D(window->GetExtent());
	}
	vk::Format Surface::GetFormat() const
	{
		return format;
	}
	vk::ColorSpaceKHR Surface::GetColorSpace() const
	{
		return colorSpace;
	}
}