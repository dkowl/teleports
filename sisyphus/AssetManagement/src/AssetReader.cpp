#include "AssetReader.h"
#include "Utils/Throw.h"
#include "Utils/PlatformMacros.h"
#ifdef SIS_WINDOWS
#include "AssetReaderUnpacked.Windows.h"
#include "AssetReaderPacked.Windows.h"
#elif defined(SIS_ANDROID)
#include "AssetReaderPacked.Android.h"
#endif

namespace Sisyphus::AssetManagement {

	std::unique_ptr<AssetReader> AssetReader::Create(ReaderType type)
	{
		switch (type) {
		case ReaderType::Unpacked:
#ifdef SIS_ANDROID
			SIS_THROW("AssetReaderUnpacked not supported on Android");
#else
			return std::make_unique<AssetReaderUnpacked>();
#endif
			break;
		case ReaderType::Packed:
			// TODO: return std::make_unique<AssetReaderPacked>();
			return nullptr;
		default:
			SIS_THROW("Unexpected ReaderType");
			return nullptr;
		}
#ifdef SIS_CLANG
		// funny how clang complains about control reaching the end without return here,
		// that's why the extra return is added
		// MSVC however does not accept it because it's unreachable
		// that's why it's #ifdefed out, even funnier
		return nullptr;
#endif
	}
}
