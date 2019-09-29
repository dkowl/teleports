#include "Asset.h"
#include <fstream>
#include "Utils/Json.h"
#include "Utils/UuidGenerator.h"

namespace AssetManagement {
	Asset::Asset(Path inPath):
		path(inPath)
	{
		metaPath = path;
		metaPath += ".meta";
		InitMetadata();
	}
	uuids::uuid Asset::GetId() const
	{
		return metadata.GetId();
	}
	void Asset::InitMetadata()
	{
		if (HasMetaFile()) {
			ReadMetaFile();
		}
		else {
			GenerateMetaFile();
		}
	}
	bool Asset::HasMetaFile()
	{
		return fs::is_regular_file(metaPath) && fs::exists(metaPath);
	}
	void Asset::ReadMetaFile()
	{
		std::ifstream metaFile(metaPath);
		if (!metaFile.is_open()) {
			throw std::runtime_error("Failed to open meta file " + metaPath.string());
		}

		json j;
		metaFile >> j;
		metadata = j.get<AssetMetadata>();
	}
	void Asset::GenerateMetadata()
	{
		String name = path.stem().string();
		uuids::uuid id = GenerateUuid();
		metadata = AssetMetadata(id, name);
	}
	void Asset::GenerateMetaFile()
	{
		GenerateMetadata();
		json j = metadata;

		std::ofstream metaFile(metaPath);
		if (!metaFile.is_open()) {
			throw std::runtime_error("Failed to create meta file " + metaPath.string());
		}
		metaFile << j;
	}
}