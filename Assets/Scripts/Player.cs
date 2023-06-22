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

    public void Init(int colorIndex)
    {
        playerColor = (PlayerColor)colorIndex;
        currentPoint = 9;
        playable = true;
        currentEdgeIndex = monopolyManager.GetBoard.GetCornerIndex((int)playerColor);
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

    }
}

public enum PlayerColor
{
    Red,
    Blue,
    Yellow,
    Green
}