#include "WindowCreator.h"
#include <conio.h>

namespace wc = WindowCreator;

int main() {
	auto window = wc::WindowCreator().CreateWindow(wc::Platform::Windows);
	_getch();
}