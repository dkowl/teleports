#pragma once
#include <unordered_map>
#include <unordered_set>
#include <stack>
#ifdef __cpp_concepts
#include <concepts>
#endif

namespace Sisyphus::Utils {

#ifdef __cpp_concepts
	template<typename Key>
	concept Hashable = requires(Key key) {
		{std::hash<Key>{}(key)}->std::convertible_to<std::size_t>;
	};
#endif

#ifdef __cpp_concepts
	template<Hashable T>
#else
	template<typename T>
#endif
	class Graph {
	public:
		void EnsureNodeExists(const T& node) {
			graph[node];
		}

		void AddEdge(const T& from, const T& to) {
			EnsureNodeExists(to);
			graph[from].insert(to);
		}

		void AddParents(const T& node, const std::vector<T>& parents) {
			EnsureNodeExists(node);
			for (auto&& parent : parents) {
				AddEdge(parent, node);
			}
		}

		bool Contains(const T& node) const {
			return graph.find(node) != graph.end();
		}

		template<typename U>
		static inline bool SetContains(const std::unordered_set<U>& set, const U& key) {
#ifdef __clang__
			return set.find(key) != set.end();
#else
			return set.contains(key);
#endif
		}

		std::vector<T> PostOrder() const {
			std::vector<T> result;
			std::unordered_set<const T*> visited;
			for (auto&& node : graph) {
				if (SetContains(visited, &node.first)) continue;

				const T* currentNode = &node.first;
				std::stack<const T*> stack;
				while(true) {
					const auto& children = graph.at(*currentNode);
					std::vector<const T*> unvisitedChildren;
					for (auto&& child : children) {
						auto childAddress = &(graph.find(child)->first);
						if (SetContains(visited, childAddress)) continue;
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
						while (SetContains(visited, stack.top())) stack.pop();
						currentNode = stack.top();
						stack.pop();
					}					
				};
			}
			return result;
		}

		void Clear() {
			graph.clear();
		}

	private:
		std::unordered_map<T, std::unordered_set<T>> graph;
	};
}