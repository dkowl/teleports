#include "WSAUtils.h"
#include <Ws2tcpip.h>
#include <stdexcept>

template<typename To, typename From>
To bit_cast(const From& from) {
	static_assert(sizeof(To) == sizeof(From));
	To to;
	std::memcpy(&to, &from, sizeof(To));
	return to;
}

struct sockaddr to_sockaddr(const SocketAddress& socketAddress)
{
	if (socketAddress.port < 0 || socketAddress.port > 65536) {
		throw std::runtime_error("to_sockaddr: Invalid port number" + std::to_string(socketAddress.port));
	}

	struct sockaddr_in result;
	result.sin_family = AF_INET;
	result.sin_port = static_cast<USHORT>(socketAddress.port);
	struct in_addr inAddr;
	inet_pton(AF_INET, socketAddress.ip.c_str(), &inAddr);
	result.sin_addr = inAddr;

	return bit_cast<struct sockaddr>(result);
}

SocketAddress from_sockaddr(const struct sockaddr& sockaddr)
{
	SocketAddress result;
	if (sockaddr.sa_family != AF_INET) {
		throw std::runtime_error("from_sockaddr: only IPv4 supported");
	}
	auto sockaddr_in = bit_cast<struct sockaddr_in>(sockaddr);

	result.port = sockaddr_in.sin_port;

	char buf[64];
	inet_ntop(AF_INET, &(sockaddr_in.sin_addr), buf, sizeof(buf));
	result.ip = std::string(buf);

	return result;
}
