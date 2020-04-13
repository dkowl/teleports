#pragma once
#include <string>
#include <sstream>
#include "Utils/PlatformMacros.h"
#ifdef SIS_ANDROID
#include <jni.h>
#endif

namespace Sisyphus {
	using String = ::std::string;

	template<typename... Args>
	String AssembleString(Args... args) {
		std::stringstream ss;
		(ss << ... << args);
		return ss.str();
	}

#ifdef SIS_ANDROID
	std::string StringFromJava(JNIEnv* env, jstring jStr);
#endif
}