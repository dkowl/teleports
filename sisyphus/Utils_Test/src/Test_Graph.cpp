#pragma once
#include "catch.hpp"
#include "Utils/Graph.h"
#include <iostream>

TEST_CASE("Graph") {

	using namespace Sisyphus::Utils;

	Graph<int> g;

	std::vector<std::pair<int, std::vector<int>>> nodes = {
		{2, {1, 10}},
		{10, {11}},
		{1, {11}},
		{3, {1}},
		{4, {1}},
		{5, {2}},
		{6, {2}},
		{7, {4}},
		{8, {7}},
		{9, {7, 4}}
	};

	for (auto&& node : nodes) {
		g.AddParents(node.first, node.second);
	}

	auto postOrder = g.PostOrder();
	for (auto&& resultNode : postOrder) {
		// Detect if the node has any children
		for (auto&& node : nodes) {
			bool childOfResultFound = false;
			for (auto&& parentNode : node.second) {
				if (parentNode == resultNode) childOfResultFound = true;
			}
			REQUIRE(!childOfResultFound);
		}

		// Remove the node
		for (int i = 0; i < nodes.size(); i++) {
			auto node = nodes[i];
			if (node.first == resultNode) {
				nodes.erase(nodes.begin() + i);
				i = -1;
			}
		}
	}

	Graph<double> g2;
	g2.AddParents(1.0, {});
	REQUIRE(g2.PostOrder() == std::vector<double>{1.0});

	Graph<char> g3;
	REQUIRE(g3.PostOrder().empty());
}

