using UnityEngine;

public class MonopolyManager : MonoBehaviour
{
    [Header("Game System")]
    [SerializeField] private Board board;
    public Board GetBoard => board;
    [SerializeField] private Dice dice;
    public Dice GetDice => dice;

    [Header("Colors")]
    public Material[] colorMaterials;

    [Header("Players")]
    [SerializeField] private GameObject pawnPrefab;
    [SerializeField] private Player currentPlayerTurn;
    public Player[] players { get; private set; }
    public int playerAmount { get; private set; }

    private void Start()
    {
        players = GetComponentsInChildren<Player>();
    }

    public void SetupPlayer(int playerAmount, int[] playerColorIndex)
    {
        // Set player amount
        this.playerAmount = playerAmount;

        // Disable player
        for (int i = 0; i < players.Length; i++)
        {
            if (i >= playerAmount)
            {
                players[i].gameObject.SetActive(false);
            }
        }

        // Set player color
        for (int i = 0; i < playerAmount; i++)
        {
            players[i].Init(playerColorIndex[i]);
        }

        // Setup board
        board.Init();

        // Spawn player pawn
        for (int i = 0; i < playerAmount; i++)
        {
            GameObject newPawn = Instantiate(pawnPrefab, board.edgeCorner[i].transform.position + Vector3.up, Quaternion.identity);
            newPawn.GetComponentInChildren<Renderer>().material = colorMaterials[(int)players[i].GetColor];
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