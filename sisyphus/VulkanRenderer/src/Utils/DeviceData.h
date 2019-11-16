#pragma once
#include "Vulkan.h"
#include "Utils/BreakAssert.h"

namespace Sisyphus::Rendering::Vulkan {
	class DeviceData {
	public:
		DeviceData(vk::Device device, vk::DeviceMemory memory, size_t sizeInBytes);
		~DeviceData();

		DeviceData(const DeviceData&) = delete;
		DeviceData& operator=(const DeviceData&) = delete;
		DeviceData(DeviceData&&) = default;
		DeviceData& operator=(DeviceData&&) = default;

		void Set(const std::byte* data);
		void Get(std::byte* data) const;

		template<typename T>
		void SetData(const T& data) {
			BreakAssert(sizeof(T) == sizeInBytes);

			Set(reinterpret_cast<const std::byte*>(&data));
		}

		template<typename T>
		void SetData(const T* data, size_t numObjects) {
			BreakAssert(sizeof(T) * numObjects == sizeInBytes);

			Set(reinterpret_cast<const std::byte*>(&data));
		}

		template<typename T>
		void SetData(const std::vector<T>& data) {
			BreakAssert(data.size() * sizeof(T) == sizeInBytes);

			Set(reinterpret_cast<const std::byte*>(data.data()));
		}

		template<typename T>
		T GetData() {
			BreakAssert(sizeof(T) == sizeInBytes);

			T result;
			Get(reinterpret_cast<std::byte*>(&result));
			return result;
		}

		template<typename T>
		std::vector<T> GetDataAsVector() {
			BreakAssert(sizeInBytes % sizeof(T) != 0);
			size_t numObjects = sizeInBytes / sizeof(T);
			std::vector<T> result;
			result.resize(numObjects);
			Get(reinterpret_cast<std::byte*>(result.data()));
			return result;
		}

	private:
		vk::Device device;
		vk::DeviceMemory memory;
		size_t sizeInBytes;

		std::byte* deviceData;
	};
}