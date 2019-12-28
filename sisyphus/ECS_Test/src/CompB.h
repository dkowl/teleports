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

	void Initialize() override;
	void RegisterEventHandlers() override;

	static bool compA_initialized;
	static bool compC_initialized;
	static bool customEventHandled;
};