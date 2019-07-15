#include "WindowsSocket.h"
#include <WinSock2.h>
#include <stdexcept>
#include <iostream>

struct WindowsSocket::PrivateData {
	SOCKET winSocket;
};

WindowsSocket::WindowsSocket():
	privateData(std::make_unique<PrivateData>())
{
	privateData->winSocket = socket(AF_INET, SOCK_DGRAM, IPPROTO_UDP);
	if (privateData->winSocket == INVALID_SOCKET) {
		throw std::runtime_error("Socket creation failed!");
	}
}

WindowsSocket::~WindowsSocket() {
	if (closesocket(privateData->winSocket)) {
		std::cerr << "closesocket failed!\n";
	}
}
