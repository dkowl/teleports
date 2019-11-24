#pragma once

namespace Sisyphus::WindowCreator {
	class WindowEvent {
	public:
		enum class Type {
			Close,
			Resize,
		};
		Type type;
	};
}