#include "WindowsNetwork.h"
#include <iostream>

int main() {
	try {
		std::unique_ptr<Network> network = std::make_unique<WindowsNetwork>();
		auto socket = network->CreateSocket();
		socket->Bind({ "127.0.0.1", 2722 });
	}
	catch (std::exception& e) {
		std::cerr << e.what() << std::endl;
	}
}