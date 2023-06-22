using UnityEngine;

[System.Serializable]
public class Edge
{
    [SerializeField] private int edgeIndex;
    [SerializeField] private EdgeType edgeType;
    [SerializeField] private EdgeColor edgeColor;
    [SerializeField] private int edgePoint = 0;

    public Edge(int edgeIndex, EdgeType edgeType)
    {
        this.edgeIndex = edgeIndex;
        this.edgeType = edgeType;
        this.edgeColor = EdgeColor.Null;
        this.edgePoint = 0;
    }

    public void SetType(EdgeType newEdgeType)
    {
        edgeType = newEdgeType;
    }
}

public enum EdgeType
{
    NormalEdge,
    CornerEdge
}

enum EdgeColor
{
    Null,
    Red,
    Blue,
    Yellow,
    Green
}