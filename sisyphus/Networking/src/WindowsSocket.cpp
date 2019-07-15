#include "WindowsSocket.h"
#include <WinSock2.h>
#include <Ws2tcpip.h>
#include <stdexcept>
#include <iostream>
#include "WSAError.h"
#include "WSAUtils.h"

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

void WindowsSocket::Connect(const SocketAddress& address)
{
	auto sockaddr = to_sockaddr(address);

	if (connect(privateData->winSocket, reinterpret_cast<const struct sockaddr*>(&sockaddr), sizeof(sockaddr))) {
		throw std::runtime_error("connecting to " + address.ip + " port " + std::to_string(address.port) + " failed!");
	}
}

void WindowsSocket::Bind(const SocketAddress& address)
{
	auto sockaddr = to_sockaddr(address);

	if (bind(privateData->winSocket, reinterpret_cast<const struct sockaddr*>(&sockaddr), sizeof(sockaddr))) {
		throw std::runtime_error("binding to " + address.ip + " port " + std::to_string(address.port) + " failed!");
	}
}

void WindowsSocket::Listen()
{
	if (listen(privateData->winSocket, SOMAXCONN)) {
		ThrowWSAError("listen failed!");
	}
}

void WindowsSocket::Send(const std::byte* data, int length)
{
	int bytesSent = send(privateData->winSocket, reinterpret_cast<const char*>(data), length, 0);
	if (bytesSent == 0) {
		throw std::runtime_error("Send failed!");
	}
	else if (bytesSent < length) {
		throw std::runtime_error("Sent only " + std::to_string(bytesSent) + " out of " + std::to_string(length) + " bytes");
	}
	std::cout << "Sent " << length << " bytes!\n";
}
