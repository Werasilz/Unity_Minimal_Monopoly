using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class MonopolyManager : MonoBehaviour
{
    [SerializeField] private BoardManager boardManager;

    [Header("Game Status")]
    [SerializeField] private bool isEndGame = false;
    public int playerAmount { get; private set; }

    [Header("Dice Settings")]
    [SerializeField] private int diceFacesAmount = 6;

    [Header("Colors")]
    public Material defaultColorMaterial;
    public Material[] colorMaterials;
    public Color32[] colors;

    [Header("Players")]
    [SerializeField] private int startPoint = 9;
    [SerializeField] private GameObject pawnPrefab;
    [SerializeField] private Player[] players;
    public Player[] GetPlayers => players;
    private int currentPlayerTurnIndex;
    public List<Player> playerLose;

    [Header("User Interface")]
    [SerializeField] private Image colorPicking;
    [SerializeField] private Image diceImage;
    [SerializeField] private Button rollButton;
    [SerializeField] private Button newGameButton;
    [SerializeField] private GameObject buyEdgeWindow;
    [SerializeField] private TextMeshProUGUI playerTurnText;
    [SerializeField] private TextMeshProUGUI diceNumberText;

    [SerializeField] private Image[] playerHUD;
    [SerializeField] private TextMeshProUGUI[] playerPointText;
    [SerializeField] private TextMeshProUGUI[] playerPointStatusText;
    [SerializeField] private TextMeshProUGUI[] rankingText;
    [SerializeField] private Image[] playerRanking;

    public void SetupPlayer(int playerAmount, ColorEnum[] selectedColors)
    {
        // Set player amount
        this.playerAmount = playerAmount;
        playerLose = new List<Player>();

        // Disable player
        for (int i = 0; i < players.Length; i++)
        {
            if (i >= playerAmount) players[i].gameObject.SetActive(false);
        }

        // Set player color
        for (int i = 0; i < playerAmount; i++)
        {
            players[i].Init(startPoint, i, selectedColors[i]);
        }

        // Setup board
        boardManager.Init();

        // Spawn player pawn
        int playerSpawnedCount = 0;

        // loop through all edges
        for (int i = 0; i < boardManager.edges.Count; i++)
        {
            // Spawn at corner edge
            if (boardManager.edges[i].edgeType == EdgeType.CornerEdge)
            {
                // Spawn pawn object
                GameObject newPawn = Instantiate(pawnPrefab, players[playerSpawnedCount].transform);
                players[playerSpawnedCount].transform.position = boardManager.edges[i].edgeObject.transform.position;
                newPawn.transform.name = "Pawn_" + players[playerSpawnedCount].playerColor.ToString();

                // Set pawn color
                int colorIndex = (int)players[playerSpawnedCount].playerColor;
                newPawn.GetComponentInChildren<Renderer>().material = colorMaterials[colorIndex];

                // Count the spawned player
                playerSpawnedCount += 1;
            }
        }

        PickFirstPlayer();
    }

    private void PickFirstPlayer()
    {
        // Show color picking ui
        colorPicking.gameObject.SetActive(true);

        // Enable player ui
        for (int i = 0; i < playerAmount; i++)
        {
            playerHUD[i].gameObject.SetActive(true);
            playerHUD[i].color = colors[(int)players[i].playerColor];
            playerPointText[i].text = "Point:" + players[i].currentPoint.ToString();
        }


        // Random first player to play
        StartCoroutine(RandomPlayer());

        IEnumerator RandomPlayer()
        {
            int lastRandom = 0;

            for (int i = 0; i < 20; i++)
            {
                // Random player
                int newRandom = Random.Range(0, playerAmount);
                if (newRandom == lastRandom) continue;
                lastRandom = newRandom;

                // Set color
                colorPicking.color = colors[(int)players[newRandom].playerColor];

                // Delay the random
                float delay = 0.15f + (i / 100);
                yield return new WaitForSeconds(delay);
            }

            // Set first player to play
            currentPlayerTurnIndex = lastRandom;
            playerTurnText.text = players[currentPlayerTurnIndex].transform.name + "'s turn";
            print(playerTurnText.text);

            yield return new WaitForSeconds(1f);

            // Setup ui
            colorPicking.gameObject.SetActive(false);
            rollButton.gameObject.SetActive(true);
        }
    }

    private void NextTurn()
    {
        // Enable roll button for next player
        rollButton.gameObject.SetActive(true);

        // Next player index
        currentPlayerTurnIndex += 1;

        // Back to first player index
        if (currentPlayerTurnIndex > playerAmount - 1) currentPlayerTurnIndex = 0;

        // Player still can play
        if (players[currentPlayerTurnIndex].playable)
        {
            playerTurnText.text = players[currentPlayerTurnIndex].transform.name + "'s turn";
            print(playerTurnText.text);
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
            // Hide roll button, show dice image
            rollButton.gameObject.SetActive(false);
            diceImage.gameObject.SetActive(true);
            int lastNumber = 0;

            for (int i = 0; i < 20; i++)
            {
                // Roll a dice
                int newNumber = Random.Range(1, diceFacesAmount + 1);
                if (newNumber == lastNumber) continue;
                lastNumber = newNumber;

                // Set number text
                diceNumberText.text = newNumber.ToString();

                // Delay the random
                float delay = 0.15f + (i / 100);
                yield return new WaitForSeconds(delay);
            }

            print(players[currentPlayerTurnIndex].transform.name + " dice = " + lastNumber);
            yield return new WaitForSeconds(1f);

            // Finished roll a dice
            diceImage.gameObject.SetActive(false);

            // Move the pawn
            players[currentPlayerTurnIndex].Move(lastNumber);

            // Wait until finished move
            while (players[currentPlayerTurnIndex].isMoving)
            {
                yield return null;
            }
        }
    }

    public void ShowBuyEdgeWindow()
    {
        buyEdgeWindow.SetActive(true);
    }

    public void BuyEdgeButton()
    {
        players[currentPlayerTurnIndex].BuyEdge();
        PassButton();
    }

    public void PassButton()
    {
        print("==========End Turn==========");
        buyEdgeWindow.SetActive(false);

        // Game is not end, go to next player's turn
        if (isEndGame == false) NextTurn();
    }

    public void UpdatePointGUI()
    {
        // Update point text to all player
        for (int i = 0; i < playerPointText.Length; i++)
        {
            playerPointText[i].text = "Point:" + players[i].currentPoint.ToString();
        }
    }

    public void Summary()
    {
        isEndGame = true;

        // Next turn is the last player
        NextTurn();
        rollButton.gameObject.SetActive(false);
        newGameButton.gameObject.SetActive(true);

        // Show ranking window
        for (int i = 0; i < playerAmount; i++)
        {
            playerRanking[i].gameObject.SetActive(true);

            if (i < playerAmount - 1)
            {
                playerRanking[i].GetComponentInChildren<TextMeshProUGUI>().text = "Lose";
                playerRanking[i].color = colors[(int)playerLose[i].playerColor];

                if (playerAmount == 2)
                {
                    rankingText[playerAmount + 1].text = "2nd";
                }
                else if (playerAmount == 3)
                {
                    rankingText[playerAmount - 1].text = "2nd";
                    rankingText[playerAmount].text = "3rd";
                }
            }
            else
            {
                playerRanking[i].color = colors[(int)players[currentPlayerTurnIndex].playerColor];

                if (playerAmount == 2)
                {
                    rankingText[i + 1].text = "1st";
                }
                else if (playerAmount == 3)
                {
                    rankingText[i - 1].text = "1st";
                }
            }
        }
    }
}