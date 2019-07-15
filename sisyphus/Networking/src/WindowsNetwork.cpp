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
	return std::make_unique<WindowsSocket>();
}

void WindowsNetwork::Initialize()
{
	WSADATA wsaData;

	if (WSAStartup(MAKEWORD(2, 2), &wsaData)) {
		throw std::runtime_error("WSAStartup failed!");
	}

}

void WindowsNetwork::Cleanup()
{
	if (WSACleanup()) {
		throw std::runtime_error("WSACleanup failed!");
	}
}
