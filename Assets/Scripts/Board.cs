using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Board
{
    [SerializeField] private MonopolyManager monopolyManager;

    [Header("Edge Settings")]
    [SerializeField] private int edgePerRow = 6;
    [SerializeField] private int maxEdgePoint = 3;

    [SerializeField] private List<Edge> edges;
    [SerializeField] private GameObject edgePrefab;
    public List<GameObject> edgeCorner { get; private set; }

    public void Init()
    {
        int maxPlayer = 4;
        int edgesAmount = (edgePerRow * maxPlayer) - maxPlayer;

        // Get corner indexes
        List<int> cornerIndexes = new List<int>();
        for (int i = 0; i < monopolyManager.playerAmount; i++)
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

        Vector3 startSpawnPosition = Vector3.zero;
        edgeCorner = new List<GameObject>();

        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < edgePerRow; j++)
            {
                // Skip first index each row
                if (i > 0 && j == 0) continue;

                // Skip last index of last row
                if (i == 3 && j == edgePerRow - 1) continue;

                // Create edge block
                GameObject newEdge = Object.Instantiate(edgePrefab, Vector3.zero, Quaternion.identity);
                newEdge.name = string.Format("Row ({0}) | Edge ({1})", i, j);

                // Set edge block color
                if (j == 0)
                {
                    int colorIndex = (int)monopolyManager.players[0].GetColor;
                    newEdge.GetComponent<Renderer>().material = monopolyManager.colorMaterials[colorIndex];
                    edgeCorner.Add(newEdge);
                }

                if (j == edgePerRow - 1 && i < monopolyManager.playerAmount - 1)
                {
                    int colorIndex = (int)monopolyManager.players[i + 1].GetColor;
                    newEdge.GetComponent<Renderer>().material = monopolyManager.colorMaterials[colorIndex];
                    edgeCorner.Add(newEdge);
                }

                // Set position
                switch (i)
                {
                    case 0:
                        newEdge.transform.position = new Vector3(startSpawnPosition.x, startSpawnPosition.y, startSpawnPosition.z + j);
                        break;
                    case 1:
                        newEdge.transform.position = new Vector3(startSpawnPosition.x + j, startSpawnPosition.y, startSpawnPosition.z);
                        break;
                    case 2:
                        newEdge.transform.position = new Vector3(startSpawnPosition.x, startSpawnPosition.y, startSpawnPosition.z - j);
                        break;
                    case 3:
                        newEdge.transform.position = new Vector3(startSpawnPosition.x - j, startSpawnPosition.y, startSpawnPosition.z);
                        break;
                }

                // Save last position for next row
                if (j == edgePerRow - 1)
                {
                    startSpawnPosition = newEdge.transform.position;
                }
            }
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