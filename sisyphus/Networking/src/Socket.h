#pragma once
#include <string>

struct SocketAddress {
	std::string ip;
	int port;
};

class Socket {
public:

	virtual ~Socket() = default;

	virtual void Connect(const SocketAddress& address) = 0;
	virtual void Bind(const SocketAddress& address) = 0;
	virtual void Listen() = 0;
	

	template<typename T>
	void Send(const T& data) {
		Send(reinterpret_cast<const std::byte*>(&data), sizeof(data));
	}
	virtual void Send(const std::byte* data, int length) = 0;

	template<typename T>
	SocketAddress ReceiveFrom(T& data) {
		return ReceiveFrom(reinterpret_cast<std::byte*>(&data), sizeof(data));
	}
	virtual SocketAddress ReceiveFrom(std::byte* data, int length) = 0;
};