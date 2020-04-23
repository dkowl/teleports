#pragma once
#include "uuid.h"
#include <string>
#include <vector>
#include "AssetMetadata.h"
#include <mutex>
#include <string_view>
#include "RawDataView.h"

namespace Sisyphus::AssetManagement {

	class Asset {
	public:
		virtual ~Asset() = default;

		virtual RawDataView Data() const = 0;
		virtual const AssetMetadata& Metadata() const = 0;
	};	
}