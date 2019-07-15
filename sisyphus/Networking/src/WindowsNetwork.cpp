#include "WindowsNetwork.h"
#include <winsock2.h>
#include <stdexcept>

WindowsNetwork::WindowsNetwork()
{
	Initialize();
}

WindowsNetwork::~WindowsNetwork()
{
	Cleanup();
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
