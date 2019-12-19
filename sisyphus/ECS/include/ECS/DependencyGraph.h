#pragma once
#include "ECS\Component.h"
#include "Utils/Graph.h"
#include <unordered_map>
#include "uuid.h"

namespace Sisyphus::ECS {

	class DependencyGraph {
	public:
		template<Component T>
		void Add() {
			for (auto&& dependency : T::Dependencies()) {
				graph.AddEdge(dependency.type, T::TypeId());
			}
		}

		std::vector<uuids::uuid> GetDestructionOrder() {
			return graph.PostOrder();
		}

	private:
		Utils::Graph<uuids::uuid> graph;
	};
}