#include "AssetReader.h"
#include "Utils/Throw.h"
#include "Utils/Platform.h"
#include "AssetReaderPacked.h"
#ifdef SIS_WINDOWS
#include "AssetReaderUnpacked.Windows.h"
#elif defined(SIS_ANDROID)
#include "AssetReaderPacked.Android.h"
#endif

namespace Sisyphus::AssetManagement {

	std::unique_ptr<AssetReader> AssetReader::Create(AssetReaderType type)
	{
		switch (type) {
		case AssetReaderType::Unpacked:
			if (CurrentPlatform() == Platform::Android) {
				SIS_THROW("AssetReaderUnpacked not supported on Android");
			}
			else {
				return std::make_unique<AssetReaderUnpacked>();
			}
			break;
		case AssetReaderType::Packed:
			return std::make_unique<AssetReaderPacked>();
		default:
			SIS_THROW("Unexpected AssetReaderType");
			return nullptr;
		}
		// compilers complain that control flow reaches the end without return...
		return nullptr;
	}
}
