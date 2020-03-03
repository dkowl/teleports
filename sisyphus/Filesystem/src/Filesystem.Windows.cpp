#include "Filesystem.h"
#include <filesystem>

namespace Sisyphus::Fs {
	bool Exists(const Path& p) {
		return std::filesystem::exists(p.String());
	}
	bool IsRegularFile(const Path& p)
	{
		return std::filesystem::is_regular_file(p.String());
	}
	bool IsDirectory(const Path& p)
	{
		return std::filesystem::is_directory(p.String());
	}
}