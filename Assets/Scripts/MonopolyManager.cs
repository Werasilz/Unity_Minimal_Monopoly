using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class MonopolyManager : MonoBehaviour
{
    [Header("Game System")]
    [SerializeField] private Board board;
    public Board GetBoard => board;
    public bool isEndGame;

    [Header("Dice")]
    [SerializeField] private int diceFacesAmount = 6;

    [Header("Colors")]
    public Material defaultColorMaterial;
    public Material[] colorMaterials;

    [Header("Players")]
    [SerializeField] private int startPoint = 9;
    [SerializeField] private GameObject pawnPrefab;
    [SerializeField] private int currentPlayerTurnIndex;
    [SerializeField] private Player[] players;
    public Player[] GetPlayers => players;
    public int playerAmount { get; private set; }
    public int playerInGame;

    [Header("User Interface")]
    [SerializeField] private Image colorPicking;
    [SerializeField] private Button rollButton;
    [SerializeField] private Image diceImage;
    [SerializeField] private TextMeshProUGUI playerTurnText;
    [SerializeField] private TextMeshProUGUI diceNumberText;
    [SerializeField] private Image[] playerHUD;
    [SerializeField] private TextMeshProUGUI[] playerPointText;

    public void SetupPlayer(int playerAmount, int[] playerColorIndex)
    {
        // Set player amount
        this.playerAmount = playerAmount;
        this.playerInGame = playerAmount;

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
            players[i].Init(startPoint, i, playerColorIndex[i]);
        }

        // Setup board
        board.Init();

        // Spawn player pawn
        int playerSpawned = 0;

        for (int i = 0; i < board.edges.Count; i++)
        {
            if (board.edges[i].edgeType == EdgeType.CornerEdge)
            {
                // Spawn pawn object
                GameObject newPawn = Instantiate(pawnPrefab, board.edges[i].edgeObject.transform.position + Vector3.up, Quaternion.identity);

                // Set parent
                players[playerSpawned].transform.position = newPawn.transform.position;
                newPawn.transform.SetParent(players[playerSpawned].transform);

                // Set color
                int colorIndex = (int)players[playerSpawned].playerColor;
                newPawn.GetComponentInChildren<Renderer>().material = colorMaterials[colorIndex];
                playerSpawned += 1;
            }
        }

        // Pick first player to play
        StartCoroutine(PickFirstPlayer());
    }

    private void NextTurn()
    {
        // Enable roll button for next player
        rollButton.gameObject.SetActive(true);

        // Next player index
        currentPlayerTurnIndex += 1;

        // Back to first player index
        if (currentPlayerTurnIndex > playerAmount - 1)
        {
            currentPlayerTurnIndex = 0;
        }

        // Player still can play
        if (players[currentPlayerTurnIndex].playable)
        {
            playerTurnText.text = "Player " + players[currentPlayerTurnIndex].playerColor.ToString() + "'s turn";
            print(players[currentPlayerTurnIndex].playerColor.ToString() + "'s turn");
        }
        // Skip lose player
        else
        {
            NextTurn();
        }
    }

    public void RollADice()
    {
        StartCoroutine(Roll());

        IEnumerator Roll()
        {
            rollButton.gameObject.SetActive(false);
            diceImage.gameObject.SetActive(true);
            int lastNumber = 0;

            for (int i = 0; i < 20; i++)
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

            print(players[currentPlayerTurnIndex].playerColor.ToString() + " dice = " + lastNumber);
            yield return new WaitForSeconds(1f);
            diceImage.gameObject.SetActive(false);

            // Move the pawn
            players[currentPlayerTurnIndex].Move(lastNumber);
            playerPointText[currentPlayerTurnIndex].text = "Point:" + players[currentPlayerTurnIndex].currentPoint.ToString();

            if (isEndGame == false)
            {
                // Next player's turn
                NextTurn();

                // Enable roll button for next player
                rollButton.gameObject.SetActive(true);
            }
        }
    }

    private IEnumerator PickFirstPlayer()
    {
        // Show color picking ui
        colorPicking.gameObject.SetActive(true);

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

        // Random first player
        int lastRandom = 0;

        for (int i = 0; i < 20; i++)
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
        currentPlayerTurnIndex = lastRandom;
        playerTurnText.text = "Player " + players[currentPlayerTurnIndex].playerColor.ToString() + "'s turn";
        print(players[currentPlayerTurnIndex].playerColor.ToString() + "'s turn");

        yield return new WaitForSeconds(1f);

        // Setup ui
        colorPicking.gameObject.SetActive(false);
        rollButton.gameObject.SetActive(true);
    }
}