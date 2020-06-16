#include "AssetManagement/AssetPacker.h"
#include "AssetManagement/AssetReader.h"
#include "Filesystem/Filesystem.h"
#include "Logger/Logger.h"

using namespace Sisyphus;
using namespace Sisyphus::AssetManagement;

int main(int argc, char** argv) {
	try {
		Fs::Path srcDir;
		if (argc > 1) {
			srcDir = std::string(argv[1]);
		}
		else {
			srcDir = "assets";
			Logger().Log("Source directory not specified, assuming default: 'assets'");
		}

		Fs::Path dstDir;
		if (argc > 2) {
			dstDir = std::string(argv[2]);
		}
		else {
			Logger().Log("Destination directory not specified, assuming default: 'assets_packed'");
			dstDir = "assets_packed";
		}

		auto reader = AssetReader::Create(AssetReaderType::Unpacked);
		reader->Read(srcDir.String());

		AssetPacker packer;
		packer.PackAssets(*reader, dstDir);
	}
	catch (std::exception& e) {
		Logger().Log("Error while packing assets: " + std::string(e.what()));
	}
	catch (...) {
		Logger().Log("Unknown error while packing assets!");
	}

	system("PAUSE");
	return 0;
}