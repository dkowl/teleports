#include "WindowsNetwork.h"
#include <winsock2.h>
#include <stdexcept>

void WindowsNetwork::Initialize()
{
	WSADATA wsaData;

	if (WSAStartup(MAKEWORD(2, 2), &wsaData)) {
		throw std::runtime_error("Winsock initialization failed!");
	}

}
