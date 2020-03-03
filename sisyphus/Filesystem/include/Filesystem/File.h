#pragma once
#include "Filesystem/Path.h"

namespace Sisyphus::Fs {
	class File {
	public:
		File(const Path& p);
		~File();
	};
}