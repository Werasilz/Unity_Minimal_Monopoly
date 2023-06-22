using UnityEngine;

public class MonopolyManager : MonoBehaviour
{
    [SerializeField] private Player[] players;
    [SerializeField] private Player currentPlayerTurn;
    [SerializeField] private Board board;
    public Board GetBoard => board;

    private void Start()
    {
        board = new Board();
        board.Init();
    }

    void PickFirstPlayer()
    {

    }

    public void NextTurn()
    {

    }

    public void CheckWinning()
    {

    }
}