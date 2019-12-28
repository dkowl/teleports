#pragma once
#include "uuid.h"
#include "ECS/Entity.h"

using namespace Sisyphus::ECS;

class CompB : public IComponent {
public:
	~CompB();

	static uuids::uuid TypeId();
	static std::string ClassName();
	static ComponentReferences Dependencies();

	static ComponentReferences WatchList(Events::Initialization);
	void HandleEvent(Events::Initialization, const uuids::uuid& type) override;

	void Initialize() override;

	static bool compA_initialized;
	static bool compC_initialized;
};