#pragma once
#include "uuid.h"
#include "Utils\FilesystemUtils.h"
#include "Utils\VectorUtils.h"
#include "AssetMetadata.h"
#include <mutex>
#include <string_view>

namespace AssetManagement {

	class Asset {
	public:
		Asset(Path inPath);

		uuids::uuid GetId() const;
		const Vector<std::byte>& GetData() const;
		const std::string_view GetDataAsString() const;

	private:
		void InitMetadata();
		bool HasMetaFile();
		void ReadMetaFile();
		void GenerateMetadata();
		void GenerateMetaFile();

		void LazyLoadData() const;
		void ReadData() const;

		Path path;
		Path metaPath;
		AssetMetadata metadata;

		mutable bool dataIsRead;
		mutable Vector<std::byte> data;
		mutable std::mutex mutex;
	};	
}