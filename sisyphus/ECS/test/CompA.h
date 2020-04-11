#pragma once
#include "uuid.h"
#include "ECS/Entity.h"
#include "CustomEvent.h"

using namespace Sisyphus::ECS;

class CompA : public IComponent {
public:
	~CompA();

	static uuids::uuid TypeId();
	static std::string ClassName();
	static ComponentReferences Dependencies();

	void Initialize() override;
	void RegisterEventHandlers() override;

	void DispatchCustomEvent();

	static bool compB_initialized;
	static bool compC_initialized;
};