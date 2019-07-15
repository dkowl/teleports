#include "WindowsNetwork.h"
#include <memory>
#include <conio.h>

int main() {
	std::unique_ptr<Network> network = std::make_unique<WindowsNetwork>();
	auto socket = network->CreateSocket();
	socket->Connect({ "127.0.0.1", 2722 });
	while (true) {
		int keyCode = _getch();
		socket->Send(keyCode);
	}
}