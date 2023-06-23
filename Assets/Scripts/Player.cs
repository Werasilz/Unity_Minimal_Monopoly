using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private MonopolyManager monopolyManager;
    [SerializeField] private BoardManager boardManager;

    [Header("Player Attributes")]
    public int currentPoint;
    public bool playable { get; private set; }
    public ColorEnum playerColor { get; private set; }
    private int currentEdgeIndex;

    public void Init(int startPoint, int playerIndex, ColorEnum color)
    {
        playerColor = color;
        currentPoint = startPoint;
        playable = true;
        transform.name = transform.name + "_" + playerColor.ToString();
        currentEdgeIndex = boardManager.GetCornerIndex(playerIndex);
    }

    public void Move(int steps)
    {
        // Set not have pawn flag to edge
        boardManager.edges[currentEdgeIndex].isHasPawn = false;

        // Next edge index
        currentEdgeIndex += steps;

        // Out of edge array
        if (currentEdgeIndex >= boardManager.edges.Count)
        {
            currentEdgeIndex = currentEdgeIndex - boardManager.edges.Count;
        }

        // Set pawn position
        print(transform.name + " move to edge " + currentEdgeIndex);
        Vector3 targetPosition = boardManager.edges[currentEdgeIndex].edgeObject.transform.position;
        transform.position = targetPosition;

        // If the edge has other player's pawn standing
        if (boardManager.edges[currentEdgeIndex].isHasPawn)
        {
            transform.position = new Vector3(transform.position.x + Random.Range(-0.3f, 0.3f), transform.position.y, transform.position.z + Random.Range(-0.3f, 0.3f));
        }

        // Set have pawn flag to edge
        boardManager.edges[currentEdgeIndex].isHasPawn = true;

        EdgeAction();
    }

    private void EdgeAction()
    {
        Edge currentEdge = boardManager.edges[currentEdgeIndex];

        switch (currentEdge.edgeType)
        {
            case EdgeType.NormalEdge:
                // Edge not have color
                if (currentEdge.edgeColor == ColorEnum.Null)
                {
                    currentEdge.BuyEdge(monopolyManager, this);
                    print(transform.name + " buy empty edge, point -1");
                }
                // Edge have same color as the player
                else if (currentEdge.edgeColor == playerColor)
                {
                    currentEdge.BuyEdge(monopolyManager, this);
                    print(transform.name + " buy own edge, edge point = " + currentEdge.edgePoint + ", point -1");
                }
                // Edge have not same color as the player
                else
                {
                    // Remove point by edge point
                    currentPoint -= currentEdge.edgePoint;
                    print(transform.name + " stand on other player's edge, point -" + currentEdge.edgePoint);

                    // Add point to the edge owner
                    foreach (Player player in monopolyManager.GetPlayers)
                    {
                        // Find player who own this edge by checking on color
                        if (player.playerColor == currentEdge.edgeColor && player.playable)
                        {
                            player.currentPoint += currentEdge.edgePoint;
                            print(player.transform.name + " point +" + currentEdge.edgePoint + " from " + transform.name);
                        }
                    }

                    // Reset edge to empty edge
                    currentEdge.Reset(monopolyManager);
                }

                CheckWinning();
                break;
            case EdgeType.CornerEdge:
                if (currentEdge.edgeColor == playerColor)
                {
                    print(transform.name + " stand on edge corner " + currentEdge.edgeColor.ToString() + ", point +3");
                    currentPoint += 3;

                }
                else
                {
                    print(transform.name + " stand on edge corner " + currentEdge.edgeColor.ToString() + ", point +1");
                    currentPoint += 1;
                }
                break;
        }

        print("==========End Turn===========");
    }

    private void CheckWinning()
    {
        // Check for lose
        if (currentPoint <= 0)
        {
            gameObject.SetActive(false);
            playable = false;
            monopolyManager.playerLose.Add(this);
            print(transform.name + " Game Over");

            // Check player in game
            if (monopolyManager.playerLose.Count == monopolyManager.playerAmount - 1)
            {
                monopolyManager.Summary();
                print("End Game");
            }
        }
    }
}

public enum ColorEnum
{
    Red,
    Blue,
    Yellow,
    Green,
    Null
}