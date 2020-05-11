#include "Path.h"
#include "cwalk.h"
#include <vector>

namespace Sisyphus::Fs {
	Path::Path()
	{
	}

	Path::Path(const char* cStr):
		pathString(cStr)
	{
	}

	Path::Path(const std::string& str) :
		pathString(str)
	{
	}

	bool Path::operator==(const Path& other)
	{
		// TODO: compare normalized paths so that 'dir/file' == 'dir\file'
		// TODO: test
		return pathString == other.pathString;
	}

	Path& Path::operator/=(const Path& p)
	{
		size_t length = cwk_path_join(CStr(), p.CStr(), nullptr, 0) + 1;
		std::vector<char> joined(length);
		cwk_path_join(CStr(), p.CStr(), joined.data(), length);
		pathString = joined.data();
		return *this;
	}

	Path Path::Stem() const
	{
		const char* filename;
		size_t filenameLength;
		cwk_path_get_basename(pathString.c_str(), &filename, &filenameLength);

		const char* extension = nullptr;
		size_t extensionLength = 0;
		cwk_path_get_extension(pathString.c_str(), &extension, &extensionLength);

		return Path(std::string(filename, filenameLength - extensionLength));
	}

	Path Path::Filename() const
	{
		const char* filename;
		size_t filenameLength;
		cwk_path_get_basename(pathString.c_str(), &filename, &filenameLength);

		return filename ? Path(std::string(filename, filenameLength)) : "";
	}

	Path Path::Dirname() const
	{
		size_t dirnameLength;
		cwk_path_get_dirname(pathString.c_str(), &dirnameLength);

		return dirnameLength ? Path(std::string(pathString.c_str(), dirnameLength)) : "";
	}

	Path Path::Extension() const
	{
		const char* extension = nullptr;
		size_t extensionLength = 0;
		cwk_path_get_extension(pathString.c_str(), &extension, &extensionLength);

		return extension ? Path(std::string(extension, extensionLength)) : "";
	}

	std::string Path::LastSegment() const
	{
		cwk_segment segment;
		bool hasSegments = cwk_path_get_last_segment(pathString.c_str(), &segment);
		if (!hasSegments) return "";
		else return std::string(segment.begin, segment.size);
	}

	bool Path::Empty() const
	{
		return pathString.empty();
	}

	const char* Path::CStr() const
	{
		return pathString.c_str();
	}

	const std::string& Path::String() const {
		return pathString;
	}

	bool Path::IsSeparator(const char* sep) {
		return cwk_path_is_separator(sep);
	}
}