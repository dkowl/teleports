#include "Throw.h"
#include <stdexcept>

namespace Sisyphus {
	void Throw(const String& message)
	{
#if defined(_DEBUG) && defined(_WIN32)
		__debugbreak();
#endif
		throw std::runtime_error(message);
	}
}
