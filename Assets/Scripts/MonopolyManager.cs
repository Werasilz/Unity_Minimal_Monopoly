using UnityEngine;

public class MonopolyManager : MonoBehaviour
{
    [SerializeField] private Board board;
    public Board GetBoard => board;
    [SerializeField] private Player[] players;
    [SerializeField] private Player currentPlayerTurn;
    private int playerAmount;

    private void Start()
    {
        board.Init();
    }

    public void SetPlayerColor(int[] playerColorIndex)
    {
        for (int i = 0; i < players.Length; i++)
        {
            players[i].Init(playerColorIndex[i]);
        }
    }

    public void SetPlayerAmount(int playerAmount)
    {
        this.playerAmount = playerAmount;

        for (int i = 0; i < players.Length; i++)
        {
            if (i >= playerAmount)
            {
                players[i].gameObject.SetActive(false);
            }
        }
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