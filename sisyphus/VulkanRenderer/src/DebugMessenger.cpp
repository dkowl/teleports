#include "DebugMessenger.h"
#include "VulkanUtils.h"
#include "Utils/Logger.h"

using namespace std::string_literals;

PFN_vkCreateDebugUtilsMessengerEXT pfnVkCreateDebugUtilsMessengerEXT;
PFN_vkDestroyDebugUtilsMessengerEXT pfnVkDestroyDebugUtilsMessengerEXT;

VKAPI_ATTR VkResult VKAPI_CALL vkCreateDebugUtilsMessengerEXT(
	VkInstance instance,
	const VkDebugUtilsMessengerCreateInfoEXT* createInfo,
	const VkAllocationCallbacks* allocator,
	VkDebugUtilsMessengerEXT* messenger) 
{
	return pfnVkCreateDebugUtilsMessengerEXT(instance, createInfo, allocator, messenger);
}

VKAPI_ATTR void VKAPI_CALL vkDestroyDebugUtilsMessengerEXT(
	VkInstance instance,
	VkDebugUtilsMessengerEXT messenger,
	const VkAllocationCallbacks* allocator)
{
	return pfnVkDestroyDebugUtilsMessengerEXT(instance, messenger, allocator);
}

namespace Sisyphus::Rendering::Vulkan {
	VkBool32 debugMessageCallback(
		VkDebugUtilsMessageSeverityFlagBitsEXT messageSeverity,
		VkDebugUtilsMessageTypeFlagsEXT messageTypes,
		const VkDebugUtilsMessengerCallbackDataEXT* callbackData,
		void* /*userData*/)
	{
		auto& logger = Logger::Get();

		logger.BeginSection(
			vk::to_string(static_cast<vk::DebugUtilsMessageSeverityFlagBitsEXT>(messageSeverity)) + ": " +
			vk::to_string(static_cast<vk::DebugUtilsMessageTypeFlagsEXT>(messageTypes)) + ":"
		);
		logger.Log("messageIdName: "s + callbackData->pMessageIdName);
		logger.Log("messageIdNumber: "s + std::to_string(callbackData->messageIdNumber));
		logger.Log("message: "s + callbackData->pMessage);
		if (callbackData->queueLabelCount > 0) {
			logger.BeginSection("Queue labels:");
			for (uint8_t i = 0; i < callbackData->queueLabelCount; i++) {
				logger.Log(callbackData->pQueueLabels[i].pLabelName);
			}
			logger.EndSection();
		}
		if (callbackData->cmdBufLabelCount > 0) {
			logger.BeginSection("Command buffer labels:");
			for (uint8_t i = 0; i < callbackData->cmdBufLabelCount; i++) {
				logger.Log(callbackData->pCmdBufLabels[i].pLabelName);
			}
			logger.EndSection();
		}
		if (callbackData->objectCount > 0) {
			for (uint8_t i = 0; i < callbackData->objectCount; i++) {
				const auto & object = callbackData->pObjects[i];
				logger.BeginSection("Object #" + std::to_string(i));
				logger.Log("type: " + vk::to_string(static_cast<vk::ObjectType>(object.objectType)));
				logger.Log("handle: " + std::to_string(object.objectHandle));
				if (object.pObjectName != nullptr) {
					logger.Log("name: " + std::string(object.pObjectName));
				}
				logger.EndSection();
			}
		}

		logger.EndSection();
		return false;
	}

	DebugMessenger::DebugMessenger(const vk::Instance& instance)
	{
		LoadFunction("vkCreateDebugUtilsMessengerEXT", instance, pfnVkCreateDebugUtilsMessengerEXT);
		LoadFunction("vkDestroyDebugUtilsMessengerEXT", instance, pfnVkDestroyDebugUtilsMessengerEXT);

		vk::DebugUtilsMessageSeverityFlagsEXT severityFlags{
			vk::DebugUtilsMessageSeverityFlagBitsEXT::eWarning |
			vk::DebugUtilsMessageSeverityFlagBitsEXT::eError |
			vk::DebugUtilsMessageSeverityFlagBitsEXT::eVerbose |
			vk::DebugUtilsMessageSeverityFlagBitsEXT::eInfo
		};

		vk::DebugUtilsMessageTypeFlagsEXT messageTypeFlags{
			vk::DebugUtilsMessageTypeFlagBitsEXT::eGeneral |
			vk::DebugUtilsMessageTypeFlagBitsEXT::ePerformance |
			vk::DebugUtilsMessageTypeFlagBitsEXT::eValidation
		};

		vk::DebugUtilsMessengerCreateInfoEXT debugUtilsMessengerCreateInfo{
			{},
			severityFlags,
			messageTypeFlags,
			&debugMessageCallback
		};

		debugUtilsMessenger = instance.createDebugUtilsMessengerEXTUnique(debugUtilsMessengerCreateInfo);
	}
}