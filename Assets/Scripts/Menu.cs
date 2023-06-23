using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] private MonopolyManager monopolyManager;
    [SerializeField] private Color32[] colors;
    [SerializeField] private Button[] selectColorButtons;
    [SerializeField] private Image[] selectColorImages;
    private ColorEnum[] selectedColors = new ColorEnum[4];
    private int playerAmount = 4;

    public void SelectPlayerAmountButton(int playerAmount)
    {
        // Set player amount to player
        this.playerAmount = playerAmount;

        for (int i = 2; i < selectColorButtons.Length; i++)
        {
            // Set button to active
            selectColorButtons[i].interactable = true;

            // More than select player amount
            if (i >= playerAmount) selectColorButtons[i].interactable = false;
        }
    }

    public void SelectColor(int playerIndex)
    {
        // Next color index
        selectedColors[playerIndex] += 1;

        // Out of color array, reset to first index
        if ((int)selectedColors[playerIndex] > 3) selectedColors[playerIndex] = 0;

        // Set image to selected color
        int colorIndex = (int)selectedColors[playerIndex];
        selectColorImages[playerIndex].color = colors[colorIndex];
    }

    public void Play()
    {
        // Check same color of players
        for (int i = 0; i < playerAmount; i++)
        {
            for (int j = 0; j < playerAmount; j++)
            {
                // Skip itself
                if (i == j) continue;

                if (selectedColors[i] == selectedColors[j])
                {
                    print("[Select Color] Can't play, there're have same color");
                    return;
                }
            }
        }

        // Setup players
        monopolyManager.SetupPlayer(playerAmount, selectedColors);

        // Disable menu
        gameObject.SetActive(false);
    }

    public void NewGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
