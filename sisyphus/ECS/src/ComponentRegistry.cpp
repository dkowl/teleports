#include "Pch_ECS.h"
#include "ECS\ComponentRegistry.h"
#include "Utils\StringUtils.h"
#include "Utils\DebugAssert.h"

namespace Sisyphus::ECS {

	namespace {
		using Registry = std::unordered_map<uuids::uuid, std::string>;
		Registry& GetRegistry() {
			static Registry registry;
			return registry;
		}
	}

	bool ComponentRegistry::Register(const uuids::uuid& typeId, const std::string& name)
	{
		SIS_DEBUGASSERT(GetRegistry().find(typeId) == GetRegistry().end());
		GetRegistry()[typeId] = name;
		return true;
	}
	std::string ComponentRegistry::GetComponentName(const uuids::uuid& typeId)
	{
		auto findResult = GetRegistry().find(typeId);
		if (findResult != GetRegistry().end()) {
			return findResult->second;
		}
		else {
			return AssembleString("Unknown component - ", typeId);
		}
	}
}
