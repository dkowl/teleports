#pragma once
#include "uuid.h"
#include <string>
#include <vector>
#include "AssetMetadata.h"
#include <mutex>
#include <string_view>

namespace Sisyphus::AssetManagement {

	class Asset {
	public:
		Asset(std::string inPath);

		uuids::uuid GetId() const;
		std::string GetName() const;
		const std::vector<std::byte>& GetData() const;
		const std::string_view GetDataAsString() const;

	private:
		void InitMetadata();
		bool HasMetaFile();
		void ReadMetaFile();
		void GenerateMetadata();
		void GenerateMetaFile();

		void LazyLoadData() const;
		void ReadData() const;

		std::string name;
		std::string path;
		std::string metaPath;
		AssetMetadata metadata;

		mutable bool dataIsRead;
		mutable std::vector<std::byte> data;
		mutable std::mutex mutex;
	};	
}