#include "Asset.h"
#include "Filesystem/Path.h"
#include "Filesystem/Filesystem.h"
#include "Utils/Json.h"
#include "Utils/Logger.h"

namespace Sisyphus::AssetManagement {
	Asset::Asset(std::string inPath, bool inReadOnly):
		path(inPath),
		metadata(inPath, inReadOnly),
		readOnly(inReadOnly),
		dataIsRead(false)
	{
	}
	const uuids::uuid& Asset::Id() const
	{
		return metadata.Id();
	}
	const std::string& Asset::Name() const
	{
		return metadata.Name();
	}
	const std::vector<std::byte>& Asset::Data() const
	{
		std::lock_guard lock(mutex);
		LazyLoadData();
		return data;
	}
	const std::string_view Asset::DataAsString() const
	{
		std::lock_guard lock(mutex);
		LazyLoadData();
		return std::string_view(reinterpret_cast<char *>(data.data()), data.size());
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
		if (metadata.IsBinary()) openmode |= std::ios::binary;
		std::ifstream file(path, openmode);

		auto size = Fs::FileSize(path);
		data.resize(size);
		file.read(reinterpret_cast<char*>(data.data()), size);

		// Windows transforms each /n into /r/n, but when not in binary mode, it does not return the /r's,
		// It takes them into account when determining the size of the file though
		// So in a text file, there is an extra /0 at the end of data for each newline in it
		if (!metadata.IsBinary()) {
			while (static_cast<uint8_t>(data.back()) == 0) data.pop_back();
		}

		dataIsRead = true;

		Logger().BeginSection("Asset file read from disk");
		Logger().Log("Id: " + uuids::to_string(metadata.Id()));
		Logger().Log("Name: " + metadata.Name());
		Logger().Log("Size: " + std::to_string(size) + " bytes");
		Logger().EndSection();
	}
}