#pragma once

namespace WindowCreator {
	class WindowEvent {
	public:
		enum class Type {
			Close,
			Resize,
		};
		Type type;
	};
}