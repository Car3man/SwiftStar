using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

public static class SwiftStar {
    public static List<Integer2> FindPath(int[,] field, Integer2 start, Integer2 goal, int clearance = 1) {
        var closedSet = new Collection<Node>();
        var openSet = new Collection<Node>();

        Node startNode = new Node(start, null, 0, GetHeuristicPathLength(start, goal));

        int[,] clearanceField = CalculateClearance(field, clearance);

        openSet.Add(startNode);
        while (openSet.Count > 0) {
            var currentNode = openSet.OrderBy(node =>
              node.EstimateFullPathLength).First();

            if (currentNode.Position.Equals(goal))
                return GetPathForNode(currentNode);

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            foreach (var neighbourNode in GetNeighbours(currentNode, goal, field, clearanceField, clearance)) {
                if (closedSet.Count(node => node.Position.Equals(neighbourNode.Position)) > 0)
                    continue;
                var openNode = openSet.FirstOrDefault(node =>
                                                      node.Position.Equals(neighbourNode.Position));

                if (openNode == null)
                    openSet.Add(neighbourNode);
                else
                  if (openNode.PathLengthFromStart > neighbourNode.PathLengthFromStart) {
                    openNode.CameFrom = currentNode;
                    openNode.PathLengthFromStart = neighbourNode.PathLengthFromStart;
                }
            }
        }
        return null;
    }

    public static int[,] CalculateClearance(int[,] field, int clearance = 1) {
        int[,] clearanceField = new int[field.GetLength(0), field.GetLength(1)];

        Debug.LogFormat("[DEBUG] ClearanceFieldSize: {0}, {1}", field.GetLength(0), field.GetLength(1));

        for (int x = 0; x < field.GetLength(0); x++) {
            for (int y = 0; y < field.GetLength(1); y++) {
                int maxClearance = 0;

                for (int s = 0; s <= clearance; s++) {
                    bool passed = true;

                    for (int xx = 0; xx < s; xx++) {
                        if (!passed) break;

                        for (int yy = 0; yy < s; yy++) {
                            if ((x + xx >= field.GetLength(0) || y + yy >= field.GetLength(1) || field[x + xx, y + yy] != 0)) {
                                passed = false;
                                break;
                            }
                        }
                    }

                    if (passed)
                        maxClearance = s;
                    else
                        break;
                }

                clearanceField[x, y] = maxClearance;
            }
        }

        return clearanceField;
    }

    private static Collection<Node> GetNeighbours(Node pathNode, Integer2 goal, int[,] field, int[,] clearanceField, int clearance = 1) {
        var result = new Collection<Node>();

        Integer2[] neighbourPoints = new Integer2[4];
        neighbourPoints[0] = new Integer2(pathNode.Position.x + 1, pathNode.Position.y);
        neighbourPoints[1] = new Integer2(pathNode.Position.x - 1, pathNode.Position.y);
        neighbourPoints[2] = new Integer2(pathNode.Position.x, pathNode.Position.y + 1);
        neighbourPoints[3] = new Integer2(pathNode.Position.x, pathNode.Position.y - 1);

        foreach (var point in neighbourPoints) {
            if (point.x < 0 || point.x >= field.GetLength(0))
                continue;
            if (point.y < 0 || point.y >= field.GetLength(1))
                continue;

            if ((field[point.x, point.y] != 0)) //check cell available
                continue;
            if (clearanceField[point.x, point.y] < clearance) { //check clearance
                continue;
            }

            var neighbourNode = new Node(point, pathNode, pathNode.PathLengthFromStart + GetDistanceBetweenNeighbours(), GetHeuristicPathLength(point, goal));
            result.Add(neighbourNode);
        }
        return result;
    }

    private static int GetDistanceBetweenNeighbours() {
        return 1;
    }

    private static List<Integer2> GetPathForNode(Node pathNode) {
        var result = new List<Integer2>();
        var currentNode = pathNode;
        while (currentNode != null) {
            result.Add(currentNode.Position);
            currentNode = currentNode.CameFrom;
        }
        result.Reverse();
        return result;
    }

    private static int GetHeuristicPathLength(Integer2 from, Integer2 to) {
        return Mathf.Abs(from.x - to.x) + Mathf.Abs(from.y - to.y);
    }
}
