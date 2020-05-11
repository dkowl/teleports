#include "AssetMetadata.h"
#include "Utils/UuidGenerator.h"
#include "Filesystem/Path.h"
#include "Utils/Json.h"
#include "Logger/Logger.h"
#include <fstream>

namespace Sisyphus::AssetManagement {
	AssetMetadata::AssetMetadata(const std::string& assetPath, bool readOnly)
	{
		std::string path = assetPath + ".meta";
		auto readStatus = ReadFromFile(path);
		if (readStatus == ReadStatus::Failed) {
			if (readOnly) {
				throw std::runtime_error("Meta file not found: " + path);
			}
			else {
				id = GenerateUuid();
				bundleId = id;
				Fs::Path fsAssetPath(assetPath);
				name = fsAssetPath.Stem().String();
				isBinary = !(Fs::Path(assetPath).Extension().String() == ".txt");

				WriteToFile(path);
			}
		}
	}

	const uuids::uuid& AssetMetadata::Id() const
	{
		return id;
	}

	const uuids::uuid& AssetMetadata::BundleId() const
	{
		return bundleId;
	}

	const std::string& AssetMetadata::Name() const
	{
		return name;
	}

	bool AssetMetadata::IsBinary() const
	{
		return isBinary;
	}

	AssetMetadata::ReadStatus AssetMetadata::ReadFromFile(const std::string& path) {
		bool readFailed = false;

		std::ifstream metaFile;
		try {
			metaFile.open(path);
		}
		catch (...) {
			readFailed = true;
		}
		if (readFailed || !metaFile.is_open()) {
			Logger().Log("Failed to open meta file " + path);
			return ReadStatus::Failed;
		}

		json j;
		metaFile >> j;
		*this = j.get<AssetMetadata>();
		return ReadStatus::Ok;
	}

	void AssetMetadata::WriteToFile(const std::string& path) const
	{
		json j = *this;

		try {
			std::ofstream metaFile(path);
			metaFile << j;
		}
		catch (...) {
			throw std::runtime_error("Failed to create meta file " + path);
		}

		Logger().Log("Meta file not found, " + path + " created");
		Logger().Log("id: " + uuids::to_string(id) + "\n");
	}
}
