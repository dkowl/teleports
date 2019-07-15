#include "WSAError.h"
#include <WinSock2.h>
#include <stdexcept>

void ThrowWSAError(std::string message) {
	int errorCode = WSAGetLastError();
	throw std::runtime_error(message + "\nError code: " + GetWSAErrorDescription(errorCode));
}

std::string GetWSAErrorDescription(int errorCode) {
	switch (errorCode) {
	case WSANOTINITIALISED:
		return "WSANOTINITIALISED";
	case WSAENETDOWN:
		return "WSAENETDOWN";
	case WSAEADDRINUSE:
		return "WSAENETDOWN";
	case WSAEINPROGRESS:
		return "WSAEINPROGRESS";
	case WSAEINVAL:
		return "WSAEINVAL";
	case WSAEISCONN:
		return "WSAEISCONN";
	case WSAEMFILE:
		return "WSAEMFILE";
	case WSAENOBUFS:
		return "WSAENOBUFS";
	case WSAENOTSOCK:
		return "WSAENOTSOCK";
	case WSAEOPNOTSUPP:
		return "WSAEOPNOTSUPP";
	default:
		return "Unknown error";
	}
}