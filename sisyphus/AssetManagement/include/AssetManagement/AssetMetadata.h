#pragma once
#include "uuid.h"
#include "json.hpp"
#include "Utils\StringUtils.h"
#include "Utils\UuidJsonSerializer.h"

namespace Sisyphus::AssetManagement {
	class AssetMetadata {
	public:
		AssetMetadata() = default;
		AssetMetadata(const std::string& assetPath, bool inReadOnly = true);

		const uuids::uuid& Id() const;
		const std::string& Name() const;
		bool IsBinary() const;

	private:
		enum class ReadStatus {
			Ok,
			Failed
		};
		ReadStatus ReadFromFile(const std::string& path);
		void WriteToFile(const std::string& path) const;

		friend struct nlohmann::adl_serializer<AssetMetadata>;
		uuids::uuid id;
		std::string name;
		bool isBinary;
	};
}

namespace nlohmann {
	template<>
	struct adl_serializer<Sisyphus::AssetManagement::AssetMetadata> {
		static void to_json(json& j, const Sisyphus::AssetManagement::AssetMetadata& metadata) {
			j["id"] = metadata.id;
			j["name"] = metadata.name;
			j["isBinary"] = metadata.isBinary;
		}
		static void from_json(const json& j, Sisyphus::AssetManagement::AssetMetadata& metadata) {
			metadata.id = j["id"].get<uuids::uuid>();
			metadata.name = j["name"];
			metadata.isBinary = j["isBinary"].get<bool>();
		}
	};
}