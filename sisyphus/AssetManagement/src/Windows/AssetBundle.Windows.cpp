#include "AssetBundle.Windows.h"
#include "Filesystem/Filesystem.h"
#include <fstream>
#include "Utils/Throw.h"

namespace Sisyphus::AssetManagement {
	bool AssetBundleWindows::ResourceExists(const std::string& path) const
	{
		return Fs::Exists(path);
	}
	std::string AssetBundleWindows::ReadHeader(const std::string& path) const
	{
		std::ifstream file(path);
		SIS_THROWASSERT_MSG(file.get() == '{', "Invalid AssetBundle header");
		int openCount = 1;
		int closeCount = 0;
		int headerLength = 1;
		while (openCount > closeCount) {
			char c = static_cast<char>(file.get());
			if (c == '{') openCount++;
			else if (c == '}') closeCount++;
			headerLength++;
		}
		file.seekg(0, file.beg);
		std::string result;
		result.resize(headerLength);
		file.read(result.data(), headerLength);
		return result;
	}
}