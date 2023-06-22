using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class MonopolyManager : MonoBehaviour
{
    [Header("Game System")]
    [SerializeField] private Board board;
    public Board GetBoard => board;

    [Header("Dice")]
    [SerializeField] private int diceFacesAmount = 6;

    [Header("User Interface")]
    [SerializeField] private Image colorPicking;
    [SerializeField] private Button rollButton;
    [SerializeField] private Image diceImage;
    [SerializeField] private TextMeshProUGUI playerTurnText;
    [SerializeField] private TextMeshProUGUI diceNumberText;
    [SerializeField] private Image[] playerHUD;
    [SerializeField] private TextMeshProUGUI[] playerPointText;

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
            newPawn.GetComponentInChildren<Renderer>().material = colorMaterials[(int)players[i].playerColor];
        }

        // Pick first player to play
        StartCoroutine(PickFirstPlayer());
    }

    private void NextTurn(Player nextPlayer)
    {
        currentPlayerTurn = nextPlayer;
        playerTurnText.text = "Player " + currentPlayerTurn.playerColor.ToString() + "'s turn";
    }

    public void RollADice()
    {
        StartCoroutine(Roll());

        IEnumerator Roll()
        {
            rollButton.gameObject.SetActive(false);
            diceImage.gameObject.SetActive(true);
            int lastNumber = 0;

            for (int i = 0; i < 50; i++)
            {
                // Roll a dice
                int newNumber = Random.Range(1, diceFacesAmount + 1);
                if (newNumber == lastNumber) continue;

                // Set number text
                diceNumberText.text = newNumber.ToString();

                // Set last number
                lastNumber = newNumber;

                // Delay the random
                float delay = 0.15f + (i / 100);
                yield return new WaitForSeconds(delay);
            }

            yield return new WaitForSeconds(1f);
            diceImage.gameObject.SetActive(false);
        }
    }

    private IEnumerator PickFirstPlayer()
    {
        // Show color picking ui
        colorPicking.gameObject.SetActive(true);
        int lastRandom = 0;

        for (int i = 0; i < 50; i++)
        {
            // Random player
            int newRandom = Random.Range(0, playerAmount);
            if (newRandom == lastRandom) continue;

            // Set color
            colorPicking.color = colorMaterials[(int)players[newRandom].playerColor].color;

            // Set last random
            lastRandom = newRandom;

            // Delay the random
            float delay = 0.15f + (i / 100);
            yield return new WaitForSeconds(delay);
        }

        // Set first player to play
        NextTurn(players[lastRandom]);

        yield return new WaitForSeconds(1f);

        // Setup ui
        colorPicking.gameObject.SetActive(false);
        rollButton.gameObject.SetActive(true);

        // Disable player ui
        for (int i = 0; i < 4; i++)
        {
            playerHUD[i].color = colorMaterials[(int)players[i].playerColor].color;

            if (i < playerAmount)
            {
                playerHUD[i].gameObject.SetActive(true);
                playerPointText[i].text = "Point:" + players[i].currentPoint.ToString();
            }
            else
            {
                playerHUD[i].gameObject.SetActive(false);
            }
        }
    }
}