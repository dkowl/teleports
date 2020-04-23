#include "AssetUnpacked.h"
#include "Filesystem/Filesystem.h"
#include "Utils/Logger.h"
#include "Utils/Throw.h"

namespace Sisyphus::AssetManagement {

	AssetUnpacked::AssetUnpacked(const Fs::Path& inPath):
		metadata(inPath.String(), false),
		path(inPath)
	{
		SIS_THROWASSERT(Fs::Exists(inPath));
	}
	RawDataView AssetUnpacked::Data() const
	{
		std::lock_guard guard(dataMutex);
		LazyLoadData();
		return RawDataView(data);
	}
	const AssetMetadata& AssetUnpacked::Metadata() const
	{
		return metadata;
	}
	void AssetUnpacked::LazyLoadData() const
	{
		if (data.Empty()) {
			ReadData();
		}
	}
	void AssetUnpacked::ReadData() const
	{
		std::ios::openmode openmode = std::ios::in;
		if (metadata.IsBinary()) openmode |= std::ios::binary;
		std::ifstream file(path.String(), openmode);

		size_t size = 0;
		if (metadata.IsBinary()) {
			size = Fs::FileSize(path);
		}
		else {
			file.ignore(std::numeric_limits<std::streamsize>::max());
			size = file.gcount();
			file.seekg(0, file.beg);
		}
		data.Init(size);
		file.read(reinterpret_cast<char*>(data.Ptr()), size);

		// Windows transforms each /n into /r/n, but when not in binary mode, it does not return the /r's,
		// It takes them into account when determining the size of the file though
		// So in a text file, there is an extra /0 at the end of data for each newline in it

		Logger().BeginSection("Asset file read from disk");
		Logger().Log("Id: " + uuids::to_string(metadata.Id()));
		Logger().Log("Name: " + metadata.Name());
		Logger().Log("Size: " + std::to_string(size) + " bytes");
		Logger().EndSection();
	}
}