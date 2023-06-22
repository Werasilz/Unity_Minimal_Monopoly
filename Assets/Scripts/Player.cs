using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private MonopolyManager monopolyManager;

    [Header("Player Attributes")]
    [SerializeField] private string playerName;
    public bool playable { get; private set; }
    public ColorEnum playerColor { get; private set; }
    public int currentPoint;
    private int currentEdgeIndex;

    public void Init(int startPoint, int playerIndex, int colorIndex)
    {
        playerColor = (ColorEnum)colorIndex;
        currentPoint = startPoint;
        playable = true;
        playerName = playerColor.ToString();
        currentEdgeIndex = monopolyManager.GetBoard.GetCornerIndex(playerIndex);
    }

    public void Move(int steps)
    {
        currentEdgeIndex += steps;

        // Out of edge array
        if (currentEdgeIndex >= monopolyManager.GetBoard.edges.Count)
        {
            int newEdgeIndex = currentEdgeIndex - monopolyManager.GetBoard.edges.Count;
            currentEdgeIndex = newEdgeIndex;
        }

        // Set pawn position
        print(playerName + " move to edge " + currentEdgeIndex);
        transform.position = monopolyManager.GetBoard.edges[currentEdgeIndex].edgeObject.transform.position + Vector3.up;

        EdgeAction();
    }

    private void EdgeAction()
    {
        Edge currentEdge = monopolyManager.GetBoard.edges[currentEdgeIndex];

        switch (currentEdge.edgeType)
        {
            case EdgeType.NormalEdge:
                // Edge not have color
                if (currentEdge.edgeColor == ColorEnum.Null)
                {
                    currentEdge.BuyEdge(monopolyManager, this);
                    CheckWinning();
                    print(playerName + " buy empty edge");
                }
                // Edge have same color as the player
                else if (currentEdge.edgeColor == playerColor)
                {
                    currentEdge.BuyEdge(monopolyManager, this);
                    CheckWinning();
                    print(playerName + " buy edge");
                }
                // Edge have not same color as the player
                else
                {
                    currentPoint -= currentEdge.edgePoint;
                    currentEdge.Reset(monopolyManager);
                    CheckWinning();
                    print(playerName + " stand on other player's edge");
                }
                break;
            case EdgeType.CornerEdge:
                if (currentEdge.edgeColor == playerColor)
                {
                    print(playerName + " stand on edge corner same color as the player");
                    currentPoint += 3;
                }
                else
                {
                    print(playerName + " stand on edge corner not same color as the player");
                    currentPoint += 1;
                }
                break;
        }

        print("===========End Turn===========");
    }

    private void CheckWinning()
    {
        // Check for lose
        if (currentPoint <= 0)
        {
            print(playerName + " Game Over");
            playable = false;
            monopolyManager.playerInGame -= 1;

            // Check player in game
            if (monopolyManager.playerInGame == 1)
            {
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