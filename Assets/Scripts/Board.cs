using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Board
{
    [SerializeField] private int edgePerRow = 6;
    [SerializeField] private int maxEdgePoint = 3;
    [SerializeField] private List<Edge> edges;

    public void Init()
    {
        int maxPlayer = 4;
        int edgesAmount = (edgePerRow * maxPlayer) - maxPlayer;

        // Get corner indexes
        List<int> cornerIndexes = new List<int>();
        for (int i = 0; i < maxPlayer; i++)
        {
            cornerIndexes.Add(GetCornerIndex(i));
        }

        // Create edges
        edges = new List<Edge>();
        for (int i = 0; i < edgesAmount; i++)
        {
            Edge newEdge = new Edge(i, cornerIndexes.Contains(i) ? EdgeType.CornerEdge : EdgeType.NormalEdge);
            edges.Add(newEdge);
        }
    }

    public int GetCornerIndex(int colorIndex)
    {
        // 2 is corner edge per row
        // edge per row - corer edge per row
        // 6 - 2 = 4 
        // normal edge per row is 4
        // multiply by color index
        return ((edgePerRow - 2) + 1) * colorIndex;
    }

    public void CheckEdge(Player player)
    {

    }

    public void BuyEdge(Player player)
    {

    }
}