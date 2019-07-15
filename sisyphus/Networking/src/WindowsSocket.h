#pragma once
#include "Socket.h"
#include <memory>

class WindowsSocket : public Socket {
public:
	WindowsSocket();
	~WindowsSocket();

	void Connect(const SocketAddress& address) override;
	void Bind(const SocketAddress& address) override;
	void Listen() override;
	void Send(const std::byte* data, int length) override;

private:

	struct PrivateData;
	std::unique_ptr<PrivateData> privateData;
};
