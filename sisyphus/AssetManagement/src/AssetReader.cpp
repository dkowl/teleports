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
#ifdef SIS_ANDROID
				SIS_THROW("AssetReaderUnpacked not supported on Android");
				return nullptr;
#else
				return std::make_unique<AssetReaderUnpacked>();
#endif
			break;
		case AssetReaderType::Packed:
			return std::make_unique<AssetReaderPacked>();
		default:
			SIS_THROW("Unexpected AssetReaderType");
			return nullptr;
		}
	}
}
