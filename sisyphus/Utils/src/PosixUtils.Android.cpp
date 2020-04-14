#include "PosixUtils.Android.h"

namespace Sisyphus {
	std::string ErrorString(int errnum) {
		char buf[256];
		return std::string(strerror_r(errnum, buf, 256));
	}
}