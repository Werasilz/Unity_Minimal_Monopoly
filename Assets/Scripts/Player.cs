using UnityEngine;
using System.Linq;

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
        // Next edge index
        currentEdgeIndex += steps;

        // Out of edge array
        if (currentEdgeIndex >= monopolyManager.GetBoard.edges.Count)
        {
            int newEdgeIndex = currentEdgeIndex - monopolyManager.GetBoard.edges.Count;
            currentEdgeIndex = newEdgeIndex;
        }

        // Set pawn position
        print(playerName + " move to edge " + currentEdgeIndex);
        Vector3 targetPosition = monopolyManager.GetBoard.edges[currentEdgeIndex].edgeObject.transform.position + Vector3.up;
        transform.position = targetPosition;

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
                    print(playerName + " buy empty edge, point -1");
                }
                // Edge have same color as the player
                else if (currentEdge.edgeColor == playerColor)
                {
                    currentEdge.BuyEdge(monopolyManager, this);

                    CheckWinning();
                    print(playerName + " buy edge, point -1");
                }
                // Edge have not same color as the player
                else
                {
                    // Remove point by edge point
                    currentPoint -= currentEdge.edgePoint;
                    print(playerName + " stand on other player's edge, point -" + currentEdge.edgePoint);

                    // Add point to edge owner
                    foreach (Player player in monopolyManager.GetPlayers)
                    {
                        if (player.playerColor == currentEdge.edgeColor && player.playable)
                        {
                            player.currentPoint += currentEdge.edgePoint;
                            print(player.playerName + " point +" + currentEdge.edgePoint + " from " + this.playerName);
                        }
                    }

                    currentEdge.Reset(monopolyManager);
                    CheckWinning();
                }
                break;
            case EdgeType.CornerEdge:
                if (currentEdge.edgeColor == playerColor)
                {
                    print(playerName + " stand on edge corner same color as the player, point +3");
                    currentPoint += 3;

                }
                else
                {
                    print(playerName + " stand on edge corner not same color as the player, point +1");
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
            gameObject.SetActive(false);
            playable = false;
            monopolyManager.playerLose.Add(this);

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