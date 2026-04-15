using System.Collections.Generic;
using UnityEngine;

namespace ASSIGNMENTS.Minecraft.Scripts {
    public class Pathfinding {
        private static readonly Vector3Int[] Directions = {
            new Vector3Int( 1, 0, 0),
            new Vector3Int(-1, 0, 0),
            new Vector3Int( 0, 0, 1),
            new Vector3Int( 0, 0,-1)
        };

        public static List<Vector3Int> findPath(Vector3Int start, Vector3Int end) {
            var openSet = new PriorityQueue<Node>();
            var closedSet = new HashSet<Vector3Int>();
            var bestG = new Dictionary<Vector3Int, float>();

            float h = heuristic(start, end);
            openSet.Enqueue(new Node(start, 0, h, null));
            bestG[start] = 0;

            int maxIter = 5000;
            int iter = 0;

            while (openSet.Count > 0 && iter < maxIter) {
                iter++;
                Node current = openSet.Dequeue();

                if (current.position == end)
                    return reconstructPath(current);
                if (closedSet.Contains(current.position)) continue;
                closedSet.Add(current.position);

                foreach (var neighbor in getNeighbors(current)) {
                    if (closedSet.Contains(neighbor.position)) continue;
                    if (!bestG.ContainsKey(neighbor.position) || neighbor.gCost < bestG[neighbor.position]) {
                        bestG[neighbor.position] = neighbor.gCost;
                        neighbor.hCost = heuristic(neighbor.position, end);
                        openSet.Enqueue(neighbor);
                    }
                }
            }
            return null;
        }

        static List<Node> getNeighbors(Node current) {
            var neighbors = new List<Node>();

            foreach (var dir in Directions) {
                Vector3Int flat = current.position + dir;

                if (isWalkable(flat)) {
                    float cost = current.gCost + 1f;
                    neighbors.Add(new Node(flat, cost, 0, current));
                    continue;
                }

                Vector3Int up = flat + Vector3Int.up;
                if (isWalkable(up)) {
                    float cost = current.gCost + 1.5f;
                    neighbors.Add(new Node(up, cost, 0, current));
                }

                Vector3Int down = flat + Vector3Int.down;
                if (isWalkable(down)) {
                    float cost = current.gCost + 1.5f;
                    neighbors.Add(new Node(down, cost, 0, current));
                }
            }
            return neighbors;
        }

        static bool isWalkable(Vector3Int pos) {
            Vector3Int below = pos + Vector3Int.down;
            return hasBlock(below) && !hasBlock(pos) && !hasBlock(pos + Vector3Int.up);
        }

        static bool hasBlock(Vector3Int pos) {
            Vector3 center = new Vector3(pos.x, pos.y, pos.z);
            Collider[] hits = Physics.OverlapBox(center, Vector3.one * 0.4f, Quaternion.identity,
                LayerMask.GetMask("Block"));
            return hits.Length > 0;
        }

        static float heuristic(Vector3Int a, Vector3Int b) {
            return Vector3.Distance(a, b);
        }

        static List<Vector3Int> reconstructPath(Node node) {
            var path = new List<Vector3Int>();
            while (node != null) {
                path.Add(node.position);
                node = node.parent;
            }
            path.Reverse();
            return path;
        }
    }
}