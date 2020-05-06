#include "ResourceLoader.h"
#include "Filesystem/Filesystem.h"
#include "Logger/Logger.h"
#include "Utils/Throw.h"

namespace Sisyphus::AssetManagement {
	struct ResourceLoader::PrivateData {
		std::string path;
		bool isBinary;
		std::ifstream file;
	};

	ResourceLoader::ResourceLoader(std::string path, bool isBinary)
	{
		SIS_THROWASSERT(Fs::Exists(path));
		std::ios::openmode openmode = std::ios::in;
		if (isBinary) openmode |= std::ios::binary;
		privateData = std::make_unique<PrivateData>(PrivateData{ path, isBinary, std::ifstream(path, openmode) });
	}

	ResourceLoader::~ResourceLoader() = default;

	std::optional<uint8_t> ResourceLoader::ReadByte() {
		auto& file = privateData->file;
		int byte = file.get();
		return file ? std::optional<uint8_t>(static_cast<uint8_t>(byte)) : std::nullopt;
	}

	ResourceLoader::LoadResult ResourceLoader::Load(RawData& data, size_t offset, size_t length) {
		auto& file = privateData->file;
		Rewind();
		size_t size = 0;
		if (privateData->isBinary) {
			size = Fs::FileSize(privateData->path);
		}
		else {
			// Windows transforms each /n into /r/n, but when not in binary mode, it does not return the /r's,
			// It takes them into account when determining the size of the file though
			// So in a text file, there is an extra /0 at the end of data for each newline in it
			file.ignore(std::numeric_limits<std::streamsize>::max());
			size = file.gcount();
			Rewind();
		}
		size -= offset;
		if (length > 0 && size > length) {
			size = length;
		}
		data.Init(size);
		file.seekg(offset, file.beg);
		file.read(reinterpret_cast<char*>(data.Ptr()), size);
		Rewind();

		return LoadResult{ true, size };	
	}

	ResourceLoader::LoadResult ResourceLoader::Load(RawData& data, std::string path, bool isBinary) {
		try {
			ResourceLoader loader(path, isBinary);
			return loader.Load(data);
		}
		catch (...) {
			Logger().Log("Error reading resource " + path);
			return LoadResult{ false, 0 };
		}
	}

	void ResourceLoader::Rewind() {
		auto& file = privateData->file;
		file.clear();
		file.seekg(0, file.beg);
	}
}