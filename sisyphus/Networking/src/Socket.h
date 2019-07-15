#pragma once
#include <string>

class Socket {
public:
	virtual ~Socket() = default;

	virtual void Connect(std::string ip, int port) = 0;
};