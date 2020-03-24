#include "Path.h"
#include "cwalk.h"

namespace Sisyphus::Fs {
	Path::Path()
	{
	}

	Path::Path(const std::string& str) :
		pathString(str)
	{
	}

	Path& Path::operator=(const std::string& str)
	{
		pathString = str;
		return *this;
	}

	Path Path::Stem() const
	{
		const char* filename;
		size_t filenameLength;
		cwk_path_get_basename(pathString.c_str(), &filename, &filenameLength);

		const char* extension;
		size_t extensionLength;
		cwk_path_get_extension(pathString.c_str(), &extension, &extensionLength);

		return Path(std::string(filename, filenameLength - extensionLength));
	}

	Path Path::Filename() const
	{
		const char* filename;
		size_t filenameLength;
		cwk_path_get_basename(pathString.c_str(), &filename, &filenameLength);

		return Path(std::string(filename, filenameLength));
	}

	Path Path::Dirname() const
	{
		size_t dirnameLength;
		cwk_path_get_dirname(pathString.c_str(), &dirnameLength);

		return Path(std::string(pathString.c_str(), dirnameLength));
	}

	Path Path::Extension() const
	{
		const char* extension;
		size_t extensionLength;
		cwk_path_get_extension(pathString.c_str(), &extension, &extensionLength);

		return Path(std::string(extension, extensionLength));
	}

	bool Path::Empty() const
	{
		return pathString.empty();
	}

	const std::string& Path::String() const {
		return pathString;
	}
}