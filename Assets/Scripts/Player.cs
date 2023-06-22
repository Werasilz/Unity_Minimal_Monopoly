using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private MonopolyManager monopolyManager;

    [Header("Player Attributes")]
    [SerializeField] private string playerName;
    [SerializeField] private bool playable;
    public PlayerColor playerColor { get; private set; }
    public int currentPoint { get; private set; }
    private int currentEdgeIndex;

    public void Init(int playerIndex, int colorIndex)
    {
        playerColor = (PlayerColor)colorIndex;
        currentPoint = 9;
        playable = true;
        playerName = playerColor.ToString();
        currentEdgeIndex = monopolyManager.GetBoard.GetCornerIndex(playerIndex);
        print(playerName + " Start at " + currentEdgeIndex);
    }

    public void RemovePoint(int removedPoint)
    {
        currentPoint -= removedPoint;
    }

    public void AddPoint(int addedPoint)
    {
        currentPoint += addedPoint;
    }

    public void Move(int steps)
    {
        currentEdgeIndex += steps;

        // Out of edge array
        if (currentEdgeIndex > monopolyManager.GetBoard.edgesBlocks.Count)
        {
            int newEdgeIndex = currentEdgeIndex - monopolyManager.GetBoard.edgesBlocks.Count;
            currentEdgeIndex = newEdgeIndex;
        }

        // Set pawn position
        transform.position = monopolyManager.GetBoard.edgesBlocks[currentEdgeIndex].transform.position + Vector3.up;
    }
}

public enum PlayerColor
{
    Red,
    Blue,
    Yellow,
    Green
}