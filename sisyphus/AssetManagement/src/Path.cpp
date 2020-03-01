#include "Pch_AssetManagement.h"
#include "Path.h"
#include "cwalk.h"

namespace Sisyphus::AssetManagement {
	Sisyphus::AssetManagement::Path::Path()
	{
	}

	Sisyphus::AssetManagement::Path::Path(const std::string& str) :
		pathString(str)
	{
	}

	Path Sisyphus::AssetManagement::Path::Stem()
	{
		const char* filename;
		size_t filenameLength;
		cwk_path_get_basename(pathString.c_str(), &filename, &filenameLength);

		const char* extension;
		size_t extensionLength;
		cwk_path_get_extension(pathString.c_str(), &extension, &extensionLength);

		return std::string(filename, filenameLength - extensionLength);
	}
}
