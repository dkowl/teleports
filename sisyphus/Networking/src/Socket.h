#pragma once
#include <string>

class Socket {
public:
	virtual ~Socket() = default;

	virtual void Connect(std::string ip, int port) = 0;

	template<typename T>
	void Send(const T& data) {
		Send(reinterpret_cast<const std::byte*>(&data), sizeof(data));
	}
	virtual void Send(const std::byte* data, int length) = 0;
};