#pragma once
#include "Socket.h"
#include <memory>

class WindowsSocket : public Socket {
public:
	WindowsSocket();
	~WindowsSocket();

private:
	struct PrivateData;
	std::unique_ptr<PrivateData> privateData;
};
