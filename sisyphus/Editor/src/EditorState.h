#pragma once
#include <vector>
#include "Filesystem/Path.h"

namespace Sisyphus::Editor {

	class EditorState {

	private:
		std::vector<Fs::Path> lastOpenedProjects;
	};

}