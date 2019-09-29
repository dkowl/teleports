#pragma once
#include "AssetMetadata.h"

namespace AssetManagement {
	AssetMetadata::AssetMetadata(uuids::uuid id, String name):
		id(id),
		name(name)
	{
	}
	uuids::uuid AssetMetadata::GetId() const
	{
		return id;
	}
	String AssetMetadata::GetName() const
	{
		return name;
	}
}
