using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] private MonopolyManager monopolyManager;
    [SerializeField] private Color32[] colors;
    [SerializeField] private Button[] selectColorButtons;
    [SerializeField] private Image[] selectColorImages;
    private int[] playerColorIndex;
    private int playerAmount;

    private void Start()
    {
        playerColorIndex = new int[4];
        playerAmount = 4;
    }

    public void SelectPlayerAmountButton(int playerAmount)
    {
        // Set player amount to player
        this.playerAmount = playerAmount;

        for (int i = 2; i < selectColorButtons.Length; i++)
        {
            // Set button to active
            selectColorButtons[i].interactable = true;

            // More than select player amount
            if (i >= playerAmount)
            {
                // Set button to inactive
                selectColorButtons[i].interactable = false;
            }
        }
    }

    public void SelectColor(int playerIndex)
    {
        // Next color index
        playerColorIndex[playerIndex] += 1;

        // Out of color array, reset to first index
        if (playerColorIndex[playerIndex] > 3)
        {
            playerColorIndex[playerIndex] = 0;
        }

        // Set image to selected color
        int colorIndex = playerColorIndex[playerIndex];
        selectColorImages[playerIndex].color = colors[colorIndex];
    }

    public void Play()
    {
        // Check same color of players
        for (int i = 0; i < playerAmount; i++)
        {
            for (int j = 0; j < playerAmount; j++)
            {
                if (i == j) continue;

                if (playerColorIndex[i] == playerColorIndex[j])
                {
                    print("[Select Color] Can't play, there're have same color");
                    return;
                }
            }
        }

        // Set color to players
        monopolyManager.SetPlayerAmount(playerAmount);
        monopolyManager.SetPlayerColor(playerColorIndex);

        // Disable menu
        gameObject.SetActive(false);
    }
}
