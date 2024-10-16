using UnityEngine;

public class CardLayoutManager : MonoBehaviour
{
    public GameObject cardPrefab; 
    public Transform gridParent; 

    public int columns = 3; 
    public int rows = 3;     

    void Start()
    {
        GenerateCards();
    }

    void GenerateCards()
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                GameObject card = Instantiate(cardPrefab, gridParent);
            }
        }
    }
}
