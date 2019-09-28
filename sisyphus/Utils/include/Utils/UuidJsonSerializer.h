#pragma once
#include "json.hpp"
#include "uuid.h"
#include "String.h"

namespace nlohmann {
	template<>
	struct adl_serializer<uuids::uuid> {
		static void to_json(json& j, const uuids::uuid& id) {
			j = uuids::to_string(id);
		}
		static void from_json(const json& j, uuids::uuid& id) {
			id.from_string(j.get<String>());
		}
	};
}