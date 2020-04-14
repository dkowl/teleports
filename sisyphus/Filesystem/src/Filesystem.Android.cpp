#include "Filesystem.h"
#include "Utils/Throw.h"
#include "Utils/PosixUtils.Android.h"
#include "Logger/Logger.h"
#include <vector>
#include <sys/stat.h>
#include <dirent.h>

namespace Sisyphus::Fs {

	namespace {
		bool CheckFileMode(const Path& p, int mode) {
			struct stat statBuf;
			int result = stat(p.CStr(), &statBuf);
			if (result == 0) return (statBuf.st_mode & S_IFMT) == mode;
			else if (errno == ENOENT) return false;
			else {
				SIS_THROW(ErrorString(errno));
			}
			return false;
		}

		struct stat GetStat(const Path& p) {
			struct stat statBuf;
			int result = stat(p.CStr(), &statBuf);
			if (result != 0) SIS_THROW(ErrorString(errno));
			return statBuf;
		}
	}

	bool Exists(const Path& p) {
		struct stat statBuf;
		int result = stat(p.CStr(), &statBuf);
		if (result == 0) return true;
		else if (errno == ENOENT) return false;
		else {
			SIS_THROW(ErrorString(errno));
		}
		return false;
	}

	bool IsRegularFile(const Path& p) {
		return CheckFileMode(p, S_IFREG);
	}

	bool IsDirectory(const Path& p) {
		return CheckFileMode(p, S_IFDIR);
	}

	uint64_t FileSize(const Path& p) {
		auto statBuf = GetStat(p);
		return statBuf.st_size;
	}

	class RecursiveDirectoryIterator::Impl {
	public:
		Impl(const Path& p) {
			auto dir = opendir(p.CStr());
			SIS_THROWASSERT_MSG(dir != nullptr, "Failed to open directory: " + p.String());
			dirs.push_back(dir);

			Increment();
		}

		void Increment() {
			// Recursive alogrithm	
			struct dirent* entry = nullptr;
			while (!dirs.empty()) {
				entry = readdir(dirs.back());
				if (entry == nullptr) {
					currentPath = currentPath.Dirname();
					dirs.pop_back();
				}
				else {
					break;
				}
			}
			if (entry) {
				AddIfDir(entry);
			}
			else { // the end
				currentPath =  "";
			}
		}

		std::vector<DIR*> dirs;
		Path currentPath;

	private:
		void AddIfDir(struct dirent* entry) {
			if (entry && entry->d_type == DT_DIR) {
				currentPath /= entry->d_name;
				dirs.push_back(opendir(currentPath.CStr()));				
			}
		}
	};

	RecursiveDirectoryIterator::RecursiveDirectoryIterator(const Path& p) :
		impl(std::make_unique<Impl>(p))
	{
	}

	RecursiveDirectoryIterator::~RecursiveDirectoryIterator() = default;

	RecursiveDirectoryIterator::RecursiveDirectoryIterator(const RecursiveDirectoryIterator& other)
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
		impl->Increment();
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
		return impl->currentPath.Empty();
	}
}