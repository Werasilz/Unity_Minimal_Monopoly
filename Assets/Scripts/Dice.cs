using UnityEngine;

public class Dice : MonoBehaviour
{
    [SerializeField] private int diceFacesAmount = 6;

    public int Roll()
    {
        int rollNumber = Random.Range(1, diceFacesAmount + 1);
        return rollNumber;
    }
}