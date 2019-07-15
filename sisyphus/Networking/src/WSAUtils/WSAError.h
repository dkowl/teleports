#pragma once
#include <string>

void ThrowWSAError(std::string message);
std::string GetWSAErrorDescription(int errorCode);
