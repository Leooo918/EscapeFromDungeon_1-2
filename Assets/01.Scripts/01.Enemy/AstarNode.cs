using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class AstarNode : IComparable<AstarNode>
{
    public int x, y;
    public AstarNode prevNode;
    public int distanceFromStart = 0;
    public int remainDistance = 0;

    public int TotalDistance => distanceFromStart + remainDistance;

    public AstarNode(Vector3Int start, Vector3Int end, Vector3Int position, AstarNode prevNode)
    {
        x = position.x;
        y = position.y;
        this.prevNode = prevNode;

        distanceFromStart = Mathf.Abs(start.x - x);
        distanceFromStart += Mathf.Abs(start.y - y);

        remainDistance = Mathf.Abs(end.x - x);
        remainDistance += Mathf.Abs(end.y - y);
    }

    public int CompareTo(AstarNode other)
    {
        return TotalDistance.CompareTo(other.TotalDistance);
    }
}
