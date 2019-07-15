#include "WindowsSocket.h"
#include <WinSock2.h>
#include <Ws2tcpip.h>
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

void WindowsSocket::Connect(std::string ip, int port)
{
	if (port < 0 || port > 65536) {
		throw std::runtime_error("Invalid port number" + std::to_string(port));
	}

	struct sockaddr_in sockaddr;
	sockaddr.sin_family = AF_INET;
	sockaddr.sin_port = port;
	struct in_addr inAddr; 
	inet_pton(AF_INET, ip.c_str(), &inAddr);
	sockaddr.sin_addr = inAddr; 

	if (connect(privateData->winSocket, reinterpret_cast<const struct sockaddr*>(&sockaddr), sizeof(sockaddr))) {
		throw std::runtime_error("connecting to " + ip + " port " + std::to_string(port) + " failed!");
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
