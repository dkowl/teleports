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
		Asset(std::string inPath, bool inReadOnly = true);

		const uuids::uuid& Id() const;
		const std::string& Name() const;
		const std::vector<std::byte>& Data() const;
		const std::string_view DataAsString() const;

	private:
		void InitMetadata();
		bool HasMetaFile();
		void ReadMetaFile();
		void GenerateMetadata();
		void GenerateMetaFile();

		void LazyLoadData() const;
		void ReadData() const;

		std::string path;
		AssetMetadata metadata;

		bool readOnly;
		mutable bool dataIsRead;
		mutable std::vector<std::byte> data;
		mutable std::mutex mutex;
	};	
}