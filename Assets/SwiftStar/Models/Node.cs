using System.Collections;
using System.Collections.Generic;

public class Node {
    public Integer2 Position;
    public int PathLengthFromStart { get; set; }
    public Node CameFrom { get; set; }
    public bool IsWall { get; set; }
    public int HeuristicEstimatePathLength { get; set; }
    public int Clearance = 1;

    public bool IsWalkable() {
        return !IsWall;
    }

    public int EstimateFullPathLength {
        get {
            return this.PathLengthFromStart + this.HeuristicEstimatePathLength;
        }
    }

    public Node(Integer2 position, Node cameFrom, int pathLengthFromStart, int heuristicEstimatePathLength) {
        Position = position;
        CameFrom = cameFrom;
        PathLengthFromStart = pathLengthFromStart;
        HeuristicEstimatePathLength = heuristicEstimatePathLength;
    }

    public override string ToString() {
        return string.Format("[Node: Position={0}", Position);
    }
}
