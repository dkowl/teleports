#pragma once
#include "Network.h"
class WindowsNetwork : public Network
{
public:
	WindowsNetwork();
	~WindowsNetwork();

	SocketPtr CreateSocket();
private:
	void Initialize();
	void Cleanup();
};

