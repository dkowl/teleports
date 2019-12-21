#pragma once
#include "uuid.h"
#include "ECS/Entity.h"

using namespace Sisyphus::ECS;

class CompA : public IComponent {
public:
	~CompA();

	static uuids::uuid TypeId();
	static std::string ClassName();
	static ComponentReferences Dependencies();

	static ComponentReferences WatchList(Events::Initialization);
	void HandleEvent(Events::Initialization, const uuids::uuid& type) override;

	void Initialize(const Entity&) override;

	const Entity* entity;

	static bool compB_initialized;
	static bool compC_initialized;
};