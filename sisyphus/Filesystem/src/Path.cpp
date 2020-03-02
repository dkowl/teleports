#include "Path.h"
#include "cwalk.h"

namespace Sisyphus::Filesystem {
	Path::Path()
	{
	}

	Path::Path(const std::string& str) :
		pathString(str)
	{
	}

	Path Path::Stem()
	{
		const char* filename;
		size_t filenameLength;
		cwk_path_get_basename(pathString.c_str(), &filename, &filenameLength);

		const char* extension;
		size_t extensionLength;
		cwk_path_get_extension(pathString.c_str(), &extension, &extensionLength);

		return Path(std::string(filename, filenameLength - extensionLength));
	}

	Path Path::Filename()
	{
		const char* filename;
		size_t filenameLength;
		cwk_path_get_basename(pathString.c_str(), &filename, &filenameLength);

		return Path(std::string(filename, filenameLength));
	}

	Path Path::Dirname()
	{
		size_t dirnameLength;
		cwk_path_get_dirname(pathString.c_str(), &dirnameLength);

		return Path(std::string(pathString.c_str(), dirnameLength));
	}

	Path Path::Extension()
	{
		const char* extension;
		size_t extensionLength;
		cwk_path_get_extension(pathString.c_str(), &extension, &extensionLength);

		return Path(std::string(extension, extensionLength));
	}

	std::string Path::String() {
		return pathString;
	}
}