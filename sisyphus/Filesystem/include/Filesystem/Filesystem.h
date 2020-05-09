#pragma once
#include "Path.h"
#include "Utils/PlatformMacros.h"
#include <memory>

namespace Sisyphus::Fs {

#ifdef SIS_ANDROID
	void Init_Android(void* env, void* javaAssetManager);
#endif

	Path CurrentPath();

	bool Exists(const Path& p);
	bool IsRegularFile(const Path& p);
	bool IsDirectory(const Path& p);

	uint64_t FileSize(const Path& p);

	class RecursiveDirectoryIterator {
		public:
			RecursiveDirectoryIterator(const Path& p);
			~RecursiveDirectoryIterator(); // default
			RecursiveDirectoryIterator(const RecursiveDirectoryIterator& other);


			const Path& operator*() const;
			const Path* operator->() const;
			RecursiveDirectoryIterator& operator++();
			RecursiveDirectoryIterator& begin();
			struct End {};
			End end() const;
			bool operator!=(End end);

		private:
			class Impl;
			std::unique_ptr<Impl> impl;
	};
}