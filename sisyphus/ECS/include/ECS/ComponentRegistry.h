#pragma once
#include <string>
#include "uuid.h"

namespace Sisyphus::ECS {

	class ComponentRegistry {
	public:
		static bool Register(const uuids::uuid& typeId, const std::string& name);

		static std::string GetComponentName(const uuids::uuid& typeId);	
	};

}