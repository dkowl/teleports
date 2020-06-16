#pragma once

namespace Sisyphus::Rendering {
	class IDrawable {
	public:
		virtual ~IDrawable() = default;
		virtual uint32_t GetVertexCount() const = 0;
		virtual uuids::uuid GetVertexShaderId() const = 0;
		virtual size_t GetVertexBufferSize() const = 0;
		virtual const std::byte* GetVertexData() const = 0;
	};
}