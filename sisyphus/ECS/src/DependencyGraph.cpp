#include "Pch_ECS.h"
#include "DependencyGraph.h"

namespace Sisyphus::ECS {
	std::vector<uuids::uuid> DependencyGraph::GetDestructionOrder() const {
		return graph.PostOrder();
	}

	void DependencyGraph::Clear() {
		graph.Clear();
	}
}