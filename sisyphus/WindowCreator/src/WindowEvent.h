#pragma once

class WindowEvent {
public:
	enum class Type {
		Close,
		Resize,
	};
	Type type;
};