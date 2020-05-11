#include <exception>
#include <iostream>
#include "Renderer\RendererFactory.h"
#include "Logger/Logger.h"
#include "AssetManagement/AssetReader.h"
#include "Renderer\IDrawable.h"
#include <android/native_activity.h>

using namespace Sisyphus;

// Android entry point
void ANativeActivity_onCreate(ANativeActivity* activity, void* savedState, size_t savedStateSize) {
	Sisyphus::Logger().Log("Hello from C++!");
}