using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    [SerializeField] private MonopolyManager monopolyManager;
    [SerializeField] private BoardManager boardManager;

    [Header("Player Attributes")]
    public int currentPoint;
    public bool isMoving { get; private set; }
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
        StartCoroutine(MoveSteps(steps, 0.15f));

        IEnumerator MoveSteps(int steps, float moveDelay)
        {
            // Set not have pawn flag to edge
            boardManager.edges[currentEdgeIndex].isHasPawn = false;

            // Store temp index
            int tempEdgeIndex = currentEdgeIndex;

            // Next edge index
            currentEdgeIndex += steps;

            // Out of edge array
            if (currentEdgeIndex >= boardManager.edges.Count)
            {
                currentEdgeIndex = currentEdgeIndex - boardManager.edges.Count;

                // If out of edge array
                // move to the last of edges (For now is index 19)
                // then move to edge index 0 and continue move by currentEdgeIndex

                print(transform.name + " move to edge " + currentEdgeIndex + " from " + tempEdgeIndex);

                // Start moving
                isMoving = true;
                Vector3 targetPosition = Vector3.zero;

                while (tempEdgeIndex <= boardManager.edges.Count - 1)
                {
                    // Move to next edge
                    targetPosition = boardManager.edges[tempEdgeIndex].edgeObject.transform.position;
                    transform.position = targetPosition;
                    yield return new WaitForSeconds(moveDelay);

                    // Next edge
                    tempEdgeIndex += 1;
                }

                // Move to next edge
                targetPosition = boardManager.edges[0].edgeObject.transform.position;
                transform.position = targetPosition;
                tempEdgeIndex = 0;
                yield return new WaitForSeconds(moveDelay);

                while (tempEdgeIndex <= currentEdgeIndex)
                {
                    // Move to next edge
                    targetPosition = boardManager.edges[tempEdgeIndex].edgeObject.transform.position;
                    transform.position = targetPosition;
                    yield return new WaitForSeconds(moveDelay);

                    // Next edge
                    tempEdgeIndex += 1;
                }

                // Finished moving
                isMoving = false;
            }
            else
            {
                print(transform.name + " move to edge " + currentEdgeIndex + " from " + tempEdgeIndex);

                // Start moving
                isMoving = true;

                while (tempEdgeIndex <= currentEdgeIndex)
                {
                    // Move to next edge
                    Vector3 targetPosition = boardManager.edges[tempEdgeIndex].edgeObject.transform.position;
                    transform.position = targetPosition;

                    yield return new WaitForSeconds(moveDelay);

                    // Next edge
                    tempEdgeIndex += 1;
                }

                // Finished moving
                isMoving = false;
            }

            Edge currentEdge = boardManager.edges[currentEdgeIndex];

            // If the edge has other player's pawn standing
            if (currentEdge.isHasPawn)
            {
                transform.position = new Vector3(transform.position.x + Random.Range(-0.3f, 0.3f), transform.position.y, transform.position.z + Random.Range(-0.3f, 0.3f));
            }

            // Set have pawn flag to edge
            currentEdge.isHasPawn = true;

            switch (currentEdge.edgeType)
            {
                case EdgeType.NormalEdge:
                    // Edge is have owner
                    if (currentEdge.edgeColor != ColorEnum.Null && currentEdge.edgeColor != playerColor)
                    {
                        // Remove point by edge point
                        currentPoint -= currentEdge.edgePoint;
                        monopolyManager.UpdatePointGUI();
                        print(transform.name + " stand on other player's edge, point -" + currentEdge.edgePoint);

                        // Reset edge to empty edge
                        currentEdge.Reset(monopolyManager);
                        CheckWinning();
                        monopolyManager.ShowBuyEdgeWindow();

                        // Add point to the edge owner
                        // foreach (Player player in monopolyManager.GetPlayers)
                        // {
                        //     // Find player who own this edge by checking on color
                        //     if (player.playerColor == currentEdge.edgeColor && player.playable)
                        //     {
                        //         player.currentPoint += currentEdge.edgePoint;
                        //         print(player.transform.name + " point +" + currentEdge.edgePoint + " from " + transform.name);
                        //     }
                        // }
                    }
                    else
                    {
                        monopolyManager.ShowBuyEdgeWindow();
                    }
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

                    // Force pass to next player
                    monopolyManager.UpdatePointGUI();
                    monopolyManager.PassButton();
                    break;
            }
        }
    }

    public void BuyEdge()
    {
        Edge currentEdge = boardManager.edges[currentEdgeIndex];

        if (currentEdge.edgeType == EdgeType.NormalEdge)
        {
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

            CheckWinning();
        }
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