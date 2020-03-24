#include "..\include\Filesystem\Filesystem.h"
#include "..\include\Filesystem\Filesystem.h"
#include "..\include\Filesystem\Filesystem.h"
#include "..\include\Filesystem\Filesystem.h"
#include "Filesystem.h"
#include <filesystem>

namespace Sisyphus::Fs {
	bool Exists(const Path& p) {
		return std::filesystem::exists(p.String());
	}
	bool IsRegularFile(const Path& p)
	{
		return std::filesystem::is_regular_file(p.String());
	}
	bool IsDirectory(const Path& p)
	{
		return std::filesystem::is_directory(p.String());
	}
	uint64_t FileSize(const Path& p) {
		return std::filesystem::file_size(p.String());
	}

	class RecursiveDirectoryIterator::Impl {
	public:
		Impl(const Path& p):
			currentPath(p),
			stdIterator(p.String())
		{
		}

		std::filesystem::recursive_directory_iterator stdIterator;
		Path currentPath;
	};

	RecursiveDirectoryIterator::RecursiveDirectoryIterator(const Path& p):
		impl(std::make_unique<Impl>(p))
	{
	}

	RecursiveDirectoryIterator::~RecursiveDirectoryIterator() = default;

	RecursiveDirectoryIterator::RecursiveDirectoryIterator(const RecursiveDirectoryIterator & other)
	{
		impl = std::make_unique<Impl>(*(other.impl));
	}

	const Path& RecursiveDirectoryIterator::operator*() const
	{
		return impl->currentPath;
	}
	const Path* RecursiveDirectoryIterator::operator->() const
	{
		return &(impl->currentPath);
	}
	RecursiveDirectoryIterator& RecursiveDirectoryIterator::operator++()
	{
		std::error_code dummy;
		impl->stdIterator.increment(dummy);
		if (impl->stdIterator != std::filesystem::recursive_directory_iterator()) {
			impl->currentPath = impl->stdIterator->path().string();
		}
		else {
			impl->currentPath = "";
		}
		return *this;
	}
	RecursiveDirectoryIterator& RecursiveDirectoryIterator::begin()
	{
		return *this;
	}
	RecursiveDirectoryIterator::End RecursiveDirectoryIterator::end() const
	{
		return End();
	}
	bool RecursiveDirectoryIterator::operator!=(End)
	{
		return impl->stdIterator != std::filesystem::recursive_directory_iterator();
	}
}