#pragma once
#include "uuid.h"
#include "Utils\FilesystemUtils.h"
#include "AssetMetadata.h"

namespace AssetManagement {
	class Asset {
	public:
		Asset(Path inPath);

		uuids::uuid GetId() const;

	private:
		void InitMetadata();
		bool HasMetaFile();
		void ReadMetaFile();
		void GenerateMetadata();
		void GenerateMetaFile();

		Path path;
		Path metaPath;
		AssetMetadata metadata;
	};	
}