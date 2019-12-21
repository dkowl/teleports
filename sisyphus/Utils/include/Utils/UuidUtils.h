#pragma once

#define SIS_DEFINE_ID(name, id) \
	static const uuids::uuid name = uuids::uuid::from_string(id).value()