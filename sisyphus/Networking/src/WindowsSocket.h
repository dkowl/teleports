#pragma once
#include "Socket.h"
#include <WinSock2.h>

class WindowsSocket : public Socket {
public:
	WindowsSocket(SOCKET winSocket);

private:
	SOCKET winSocket;
};
