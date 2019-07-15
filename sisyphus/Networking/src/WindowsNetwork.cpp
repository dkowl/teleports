#include "WindowsNetwork.h"
#include <winsock2.h>
#include <stdexcept>
#include <string>
#include "WindowsSocket.h"

WindowsNetwork::WindowsNetwork()
{
	Initialize();
}

WindowsNetwork::~WindowsNetwork()
{
	Cleanup();
}

Network::SocketPtr WindowsNetwork::CreateSocket()
{
	SOCKET createdSocket = socket(AF_INET, SOCK_DGRAM, IPPROTO_UDP);
	if (createdSocket == INVALID_SOCKET) {
		throw std::runtime_error("Socket creation failed");
	}
	return std::make_unique<WindowsSocket>(createdSocket);
}

void WindowsNetwork::Initialize()
{
	WSADATA wsaData;

	if (WSAStartup(MAKEWORD(2, 2), &wsaData)) {
		throw std::runtime_error("Winsock initialization failed!");
	}

}

void WindowsNetwork::Cleanup()
{
	if (WSACleanup()) {
		throw std::runtime_error("WSACleanup failed!");
	}
}
