using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CardLayoutManager : MonoBehaviour
{
    public GameObject cardPrefab;
    public Transform gridParent;

    public int columns = 3;
    public int rows = 3;

    public Sprite[] availableSprites;

    private List<Sprite> selectedSprites;
    private List<Sprite> randomizedSprites; 


    void Start()
    {
        GenerateCards();
    }

    // Generates cards based on rows and columns
    void GenerateCards()
    {
        int totalCards = columns * rows;

        // Ensure there is an even number of cards for matching pairs
        if (totalCards % 2 != 0)
        {
            Debug.LogError("The total number of cards must be an even number for matching pairs.");
            return;
        }

        // Step 1: Select half the total number of sprites for pairs
        selectedSprites = new List<Sprite>();

        for (int i = 0; i < totalCards / 2; i++)
        {
            if (i < availableSprites.Length)
            {
                selectedSprites.Add(availableSprites[i]);
            }
        }

        // Step 2: Duplicate the selected sprites to create pairs
        randomizedSprites = new List<Sprite>(selectedSprites);
        randomizedSprites.AddRange(selectedSprites); // Add a second set of the same sprites

        
        Shuffle(randomizedSprites);

        // Step 4: Instantiate cards and assign the randomized sprites
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                GameObject card = Instantiate(cardPrefab, gridParent);

                CardController cardController = card.GetComponent<CardController>();

                // Assign the front image sprite from the shuffled list
                int spriteIndex = i * columns + j;
                if (spriteIndex < randomizedSprites.Count)
                {
                    cardController.frontImage.sprite = randomizedSprites[spriteIndex];
                }
            }
        }
    }

    // Fisher-Yates Shuffle algorithm to randomize the list
    void Shuffle(List<Sprite> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            Sprite temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}
