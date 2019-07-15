#include "WindowsNetwork.h"
#include <memory>

int main() {
	std::unique_ptr<Network> network = std::make_unique<WindowsNetwork>();
	auto socket = network->CreateSocket();
	socket->Connect("127.0.0.1", 2721);
	socket->Send(1000);
}