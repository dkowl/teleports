#pragma once
#include "uuid.h"
#include "ECS/Entity.h"

using namespace Sisyphus::ECS;

class CompC : public IComponent {
public:
	static uuids::uuid TypeId();
	static std::string ClassName();
	static ComponentReferences Dependencies();

	void Initialize() override;

	bool initialized = false;
};