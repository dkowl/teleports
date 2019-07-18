#include "WindowsWindow.h"
#include <conio.h>

int main() {
	std::unique_ptr<Window> window = std::make_unique<WindowsWindow>();
	_getch();
}