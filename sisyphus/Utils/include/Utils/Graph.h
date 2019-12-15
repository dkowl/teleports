#pragma once
#include <unordered_map>
#include <unordered_set>
#include <stack>
#include <concepts>

namespace Sisyphus::Utils {

	template<typename Key>
	concept Hashable = requires(Key key) {
		{std::hash<Key>{}(key)}->std::convertible_to<std::size_t>;
	};

	template<typename T>
	class Graph {
	public:
		void AddParents(const T& node, const std::vector<T>& parents) {
			graph[node];
			for (auto&& parent : parents) {
				graph[parent].insert(node);
			}
		}

		std::vector<T> PostOrder() {
			std::vector<T> result;
			std::unordered_set<const T*> visited;
			for (auto&& node : graph) {
				if (visited.contains(&node.first)) continue;

				const T* currentNode = &node.first;
				std::stack<const T*> stack;
				while(true) {
					const auto& children = graph[*currentNode];
					std::vector<const T*> unvisitedChildren;
					for (auto&& child : children) {
						auto childAddress = &(graph.find(child)->first);
						if (visited.contains(childAddress)) continue;
						unvisitedChildren.push_back(childAddress);
					}

					if (unvisitedChildren.empty()) {
						result.push_back(*currentNode);
						visited.insert(currentNode);
					}
					else {
						stack.push(currentNode);
						for (auto&& unvisitedChild : unvisitedChildren) {
							stack.push(unvisitedChild);
						}
					}

					if (stack.empty()) {
						break;
					}
					else {
						while (visited.contains(stack.top())) stack.pop();
						currentNode = stack.top();
						stack.pop();
					}					
				};
			}
			return result;
		}
	private:
		std::unordered_map<T, std::unordered_set<T>> graph;
	};
}