#include "Pch_AssetManagement.h"
#include "Asset.h"
#include "Utils/FilesystemUtils.h"
#include "Utils/Json.h"
#include "Utils/UuidGenerator.h"
#include "Utils/Logger.h"

namespace Sisyphus::AssetManagement {
	Asset::Asset(std::string inPath):
		path(inPath),
		dataIsRead(false)
	{
		Path fsPath(path);
		if (!fs::is_regular_file(fsPath)) {
			throw std::runtime_error("Asset file cannot be a directory");
		}
		else if (!fs::exists(fsPath)) {
			throw std::runtime_error("Asset file " + path + " does not exist");
		}
		name = fsPath.stem().string();

		metaPath = path;
		metaPath += ".meta";
		InitMetadata();
	}
	uuids::uuid Asset::GetId() const
	{
		return metadata.id;
	}
	std::string Asset::GetName() const
	{
		return name;
	}
	const std::vector<std::byte>& Asset::GetData() const
	{
		std::lock_guard lock(mutex);
		LazyLoadData();
		return data;
	}
	const std::string_view Asset::GetDataAsString() const
	{
		std::lock_guard lock(mutex);
		LazyLoadData();
		return std::string_view(reinterpret_cast<char *>(data.data()), data.size());
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
		Path fsMetaPath(metaPath);
		return fs::is_regular_file(fsMetaPath) && fs::exists(fsMetaPath);
	}
	void Asset::ReadMetaFile()
	{
		std::ifstream metaFile(metaPath);
		if (!metaFile.is_open()) {
			throw std::runtime_error("Failed to open meta file " + metaPath);
		}

		json j;
		metaFile >> j;
		metadata = j.get<AssetMetadata>();
	}
	void Asset::GenerateMetadata()
	{
		metadata.id = GenerateUuid();		
		metadata.name = name;
		metadata.isBinary = !(Path(path).extension() == ".txt");
	}
	void Asset::GenerateMetaFile()
	{
		GenerateMetadata();
		json j = metadata;

		std::ofstream metaFile(metaPath);
		if (!metaFile.is_open()) {
			throw std::runtime_error("Failed to create meta file " + metaPath);
		}
		metaFile << j;

		Logger::Get().Log("Meta file not found, " + metaPath + " created");
		Logger::Get().Log("id: " + uuids::to_string(metadata.id) + "\n");
	}
	void Asset::LazyLoadData() const
	{
		if (!dataIsRead) {
			ReadData();
		}
	}
	void Asset::ReadData() const
	{
		std::ios::openmode openmode = std::ios::in;
		if (metadata.isBinary) openmode |= std::ios::binary;
		std::ifstream file(path, openmode);

		auto size = fs::file_size(path);
		data.resize(size);
		file.read(reinterpret_cast<char*>(data.data()), size);

		// Windows transforms each /n into /r/n, but when not in binary mode, it does not return the /r's,
		// It takes them into account when determining the size of the file though
		// So in a text file, there is an extra /0 at the end of data for each newline in it
		if (!metadata.isBinary) {
			while (static_cast<uint8_t>(data.back()) == 0) data.pop_back();
		}

		dataIsRead = true;

		Logger::Get().BeginSection("Asset file read from disk");
		Logger::Get().Log("Id: " + uuids::to_string(metadata.id));
		Logger::Get().Log("Name: " + metadata.name);
		Logger::Get().Log("Size: " + std::to_string(size) + " bytes");
		Logger::Get().EndSection();
	}
}