#pragma once
#include "Socket.h"
#include <memory>

class WindowsSocket : public Socket {
public:
	WindowsSocket();
	~WindowsSocket();

	void Connect(std::string ip, int port);

private:
	struct PrivateData;
	std::unique_ptr<PrivateData> privateData;
};
