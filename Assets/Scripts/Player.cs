using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private MonopolyManager monopolyManager;

    [Header("Player Attributes")]
    [SerializeField] private PlayerColor playerColor;
    [SerializeField] private string playerName;
    [SerializeField] private int currentPoint = 9;
    private bool playable = true;
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

enum PlayerColor
{
    Red,
    Blue,
    Yellow,
    Green
}