#include "UuidGenerator.h"
#include <random>
#include <array>
#include <algorithm>

namespace Sisyphus {
	namespace {
		uuids::uuid_random_generator InitGenerator() {
			std::random_device rd;
			auto seedData = std::array<int, std::mt19937::state_size>();
			std::generate(std::begin(seedData), std::end(seedData), std::ref(rd));
			std::seed_seq seq(std::begin(seedData), std::end(seedData));
			std::mt19937 generator(seq);
			return uuids::uuid_random_generator{ generator };
		}
	}

	uuids::uuid GenerateUuid()
	{
		static auto generator = InitGenerator();
		return generator();
	}
}
