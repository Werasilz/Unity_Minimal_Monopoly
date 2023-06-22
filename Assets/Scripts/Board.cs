using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Board
{
    [SerializeField] private MonopolyManager monopolyManager;

    [Header("Edge Settings")]
    [SerializeField] private int edgePerRow = 6;
    [SerializeField] private int maxEdgePoint = 3;
    [SerializeField] private GameObject edgePrefab;
    [SerializeField] private GameObject edgeCornerPrefab;
    public List<Edge> edges { get; private set; }

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
        Vector3 startSpawnPosition = Vector3.zero;

        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < edgePerRow; j++)
            {
                // Skip first index each row
                if (i > 0 && j == 0) continue;

                // Skip last index of last row
                if (i == 3 && j == edgePerRow - 1) continue;

                // Create edge
                GameObject newEdgeObject;
                Edge newEdge = new Edge(maxEdgePoint);

                // Set edge block color (corner) 
                if (j == 0)
                {
                    newEdgeObject = Object.Instantiate(edgeCornerPrefab, Vector3.zero, Quaternion.identity);
                    newEdgeObject.name = string.Format("Row ({0}) | Edge ({1}) | Corner", i, j);

                    int colorIndex = (int)monopolyManager.GetPlayers[0].playerColor;
                    newEdgeObject.GetComponent<Renderer>().material = monopolyManager.colorMaterials[colorIndex];

                    newEdge.SetType(EdgeType.CornerEdge, monopolyManager.GetPlayers[0].playerColor);

                }
                else if (j == edgePerRow - 1 && i < monopolyManager.playerAmount - 1)
                {
                    newEdgeObject = Object.Instantiate(edgeCornerPrefab, Vector3.zero, Quaternion.identity);
                    newEdgeObject.name = string.Format("Row ({0}) | Edge ({1}) | Corner", i, j);

                    int colorIndex = (int)monopolyManager.GetPlayers[i + 1].playerColor;
                    newEdgeObject.GetComponent<Renderer>().material = monopolyManager.colorMaterials[colorIndex];

                    newEdge.SetType(EdgeType.CornerEdge, monopolyManager.GetPlayers[i + 1].playerColor);
                }
                else
                {
                    newEdgeObject = Object.Instantiate(edgePrefab, Vector3.zero, Quaternion.identity);
                    newEdgeObject.name = string.Format("Row ({0}) | Edge ({1})", i, j);

                    newEdge.SetType(EdgeType.NormalEdge, ColorEnum.Null);
                }

                // Set parent
                newEdgeObject.transform.SetParent(monopolyManager.transform);

                // Set position
                switch (i)
                {
                    case 0:
                        newEdgeObject.transform.position = new Vector3(startSpawnPosition.x, startSpawnPosition.y, startSpawnPosition.z + j);
                        break;
                    case 1:
                        newEdgeObject.transform.position = new Vector3(startSpawnPosition.x + j, startSpawnPosition.y, startSpawnPosition.z);
                        break;
                    case 2:
                        newEdgeObject.transform.position = new Vector3(startSpawnPosition.x, startSpawnPosition.y, startSpawnPosition.z - j);
                        break;
                    case 3:
                        newEdgeObject.transform.position = new Vector3(startSpawnPosition.x - j, startSpawnPosition.y, startSpawnPosition.z);
                        break;
                }

                // Save last position for next row
                if (j == edgePerRow - 1)
                {
                    startSpawnPosition = newEdgeObject.transform.position;
                }

                // Add to list
                newEdge.SetObject(newEdgeObject);
                edges.Add(newEdge);
            }
        }
    }

    public int GetCornerIndex(int playerIndex)
    {
        // 2 is corner edge per row
        // edge per row - corer edge per row
        // 6 - 2 = 4 
        // normal edge per row is 4
        // multiply by player index
        return ((edgePerRow - 2) + 1) * playerIndex;
    }
}