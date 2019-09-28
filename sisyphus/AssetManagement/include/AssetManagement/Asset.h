#pragma once
#include "uuid.h"
#include "Utils\Filesystem.h"
#include "AssetMetadata.h"

namespace AssetManagement {
	class Asset {
	public:
		Asset(Path inPath);

	private:
		void InitMetadata();
		bool HasMetaFile();
		void ReadMetaFile();
		void GenerateMetaFile();

		Path path;
		Path metaPath;
		AssetMetadata metadata;
	};	
}