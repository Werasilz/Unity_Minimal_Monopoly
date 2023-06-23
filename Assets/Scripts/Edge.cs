using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Edge
{
    public EdgeType edgeType { get; private set; }
    public ColorEnum edgeColor { get; private set; }
    public GameObject edgeObject { get; private set; }
    private List<GameObject> edgePointObject;

    public bool isHasPawn;
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

        for (int i = 0; i < edgePointObject.Count; i++)
        {
            edgePointObject[i].SetActive(false);
        }
    }

    public void SetObject(GameObject newObject)
    {
        edgeObject = newObject;
        edgePointObject = new List<GameObject>();

        foreach (Transform child in edgeObject.transform)
        {
            edgePointObject.Add(child.gameObject);
        }
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
            edgeObject.GetComponent<Renderer>().material = monopolyManager.colorMaterials[(int)edgeColor];
        }

        // Subtract player's point
        player.currentPoint -= 1;
        monopolyManager.UpdatePointGUI();
        monopolyManager.NoticeMessage(player.transform.name + " point -1");
        if (player.currentPoint < 0) player.currentPoint = 0;

        edgePointObject[edgePoint].SetActive(true);
        edgePoint += 1;
    }
}

public enum EdgeType
{
    NormalEdge,
    CornerEdge
}