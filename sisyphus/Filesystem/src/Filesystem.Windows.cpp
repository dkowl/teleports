#include "Filesystem.h"
#include <filesystem>

namespace Sisyphus::Fs {
	Path CurrentPath()
	{
		return std::filesystem::current_path().string();
	}
	void CreateDirectories(const Path& p)
	{
		std::filesystem::create_directories(p.String());
	}
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
			stdIterator(p.String())
		{
			UpdateCurrentPath();
		}

		void UpdateCurrentPath() {
			if (stdIterator != std::filesystem::recursive_directory_iterator()) {
				currentPath = stdIterator->path().string();
			}
			else {
				currentPath = "";
			}
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
		impl->UpdateCurrentPath();
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