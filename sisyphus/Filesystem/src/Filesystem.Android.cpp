#include "Filesystem.h"

namespace Sisyphus::Fs {
	bool Exists(const Path& p) {
		return !p.String().empty();
	}
}