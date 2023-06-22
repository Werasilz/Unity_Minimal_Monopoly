using UnityEngine;

[System.Serializable]
public class Edge
{
    public EdgeType edgeType { get; private set; }
    public ColorEnum edgeColor { get; private set; }
    public GameObject edgeObject { get; private set; }
    private GameObject[] edgePointObject;
    public int edgePoint { get; private set; }
    private int maxEdgePoint;

    public Edge(int maxEdgePoint)
    {
        this.maxEdgePoint = maxEdgePoint;
        this.edgeType = edgeType;
        this.edgePoint = 0;
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
        edgePointObject = edgeObject.transform.GetComponentsInChildren<GameObject>();
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
        edgePointObject[edgePoint - 1].SetActive(true);
    }
}

public enum EdgeType
{
    NormalEdge,
    CornerEdge
}