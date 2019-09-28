#pragma once
#include "AssetMetadata.h"

namespace AssetManagement {
	uuids::uuid AssetMetadata::GetId() const
	{
		return id;
	}
	String AssetMetadata::GetName() const
	{
		return name;
	}
}
