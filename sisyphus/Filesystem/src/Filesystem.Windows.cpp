#include "Filesystem.h"
#include <filesystem>

namespace Sisyphus::Fs {
	bool Exists(const Path& p) {
		return std::filesystem::exists(p.String());
	}
}