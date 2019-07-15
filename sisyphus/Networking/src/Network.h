#pragma once
#include <memory>
#include "Socket.h"
class Network
{
public:
	virtual ~Network() = default;

	using SocketPtr = std::unique_ptr<Socket>;
	virtual SocketPtr CreateSocket() = 0;
};

