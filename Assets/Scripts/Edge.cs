using UnityEngine;

[System.Serializable]
public class Edge
{
    public EdgeType edgeType { get; private set; }
    public ColorEnum edgeColor { get; private set; }
    private int maxEdgePoint;
    public int edgePoint { get; private set; }
    public GameObject edgeObject { get; private set; }

    public Edge(int maxEdgePoint)
    {
        this.maxEdgePoint = maxEdgePoint;
        this.edgeType = edgeType;
        this.edgePoint = 0;
        // this.edgePosition = Vector3.zero;
    }

    public void Reset(MonopolyManager monopolyManager)
    {
        this.edgeColor = ColorEnum.Null;
        this.edgePoint = 0;
        edgeObject.GetComponent<Renderer>().material = monopolyManager.defaultColorMaterial;
    }

    public void SetObject(GameObject newObject)
    {
        edgeObject = newObject;
    }

    public void SetType(EdgeType newEdgeType, ColorEnum color)
    {
        edgeType = newEdgeType;
        edgeColor = color;
    }

    public void BuyEdge(MonopolyManager monopolyManager, Player player)
    {
        if (edgePoint > maxEdgePoint) return;

        if (edgeColor == ColorEnum.Null)
        {
            edgeColor = player.playerColor;
            edgeObject.GetComponent<Renderer>().material = monopolyManager.colorMaterials[(int)player.playerColor];
        }

        player.currentPoint -= 1;
        edgePoint += 1;
    }
}

public enum EdgeType
{
    NormalEdge,
    CornerEdge
}