#include "WindowsNetwork.h"
#include <memory>

int main() {
	std::unique_ptr<Network> network = std::make_unique<WindowsNetwork>();
}