#include "Asset.h"
#include "Utils/Json.h"
#include <fstream>

namespace AssetManagement {
	Asset::Asset(Path inPath):
		path(inPath)
	{
		metaPath = path;
		metaPath += ".meta";
		InitMetadata();
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
		json j;
		metaFile >> j;
		metadata = j.get<AssetMetadata>();
	}
	void Asset::GenerateMetaFile()
	{
		// TODO:
	}
}