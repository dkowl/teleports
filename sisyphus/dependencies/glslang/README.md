Glslang binaries are not tracked.

#What to do:

Clone this repo: https://github.com/KhronosGroup/glslang

Also run ./update_glslang_sources.py for SPIRV-Tools

Then just CMake for (Windows/Android) x (Debug/Release) 4 options total

Example CMake for Android (don't worry about the others, they are described in the glslang repo's readme):

Android/Debug: 
cmake $SOURCE_DIR -G "Unix Makefiles" -DANDROID_ABI=arm64-v8a -DCMAKE_BUILD_TYPE=Debug -DANDROID_STL=c++_static -DANDROID_PLATFORM=android-24 -DCMAKE_SYSTEM_NAME=Android -DANDROID_TOOLCHAIN=clang -DANDROID_ARM_MODE=arm -DCMAKE_MAKE_PROGRAM="$ANDROID_NDK_ROOT\prebuilt\windows-x86_64\bin\make.exe" -DCMAKE_TOOLCHAIN_FILE="$ANDROID_NDK_ROOT/build/cmake/android.toolchain.cmake"

$SOURCE_DIR and $ANDROID_NDK_ROOT are variables, set them in powershell like this: $VAR_NAME = "the_path"

$SOURCE_DIR is usually ../.. when you run from a build-android/debug directory or something similar, using build/(platform)/(config) in the future could be even better

$ANDROID_NDK_ROOT - you can (should?) use the one bundled with Visual Studio, mine was at: C:\\Microsoft\AndroidNDK64\android-ndk-r16b

#Why?:

These files are over 2GB, so 
1. they would exhaust the free github LFS sotrage quate and cost 5$/month, not a big deal but why waste money when you don't have to
2. this would increase total repo size considerably

Downsides:
1. you need to do these manual steps
2. at some point in time having a concrete version of glslang might become important, then we might consider building a specific version and putting that on LFS



