#pragma once
#include "Network.h"
class WindowsNetwork : public Network
{
public:
	WindowsNetwork();
	~WindowsNetwork();
private:
	void Initialize();
	void Cleanup();
};

