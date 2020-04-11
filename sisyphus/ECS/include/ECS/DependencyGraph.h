#pragma once
#include "ECS\Component.h"
#include "Utils/Graph.h"
#include <unordered_map>
#include "uuid.h"

namespace Sisyphus::ECS {

	class DependencyGraph {
	public:
#ifdef __cpp_concepts
		template<Component T>
#else
		template<typename T>
#endif
		void Add() {
			for (auto&& dependency : T::Dependencies()) {
				graph.AddEdge(dependency.type, T::TypeId());
			}
		}

		std::vector<uuids::uuid> GetDestructionOrder() const;
		void Clear();

	private:
		Utils::Graph<uuids::uuid> graph;
	};
}