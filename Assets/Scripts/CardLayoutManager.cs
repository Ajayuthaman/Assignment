using UnityEngine;
using System.Collections.Generic;

public class CardLayoutManager : MonoBehaviour
{
    public GameObject cardPrefab;
    public Transform gridParent;

    private List<Sprite> randomizedSprites;

    // Called to set up the current level
    public void SetUpLevel(LevelData levelData)
    {
        // Update rows, columns, and available sprites based on the LevelData
        int columns = levelData.columns;
        int rows = levelData.rows;
        Sprite[] availableSprites = levelData.levelImages;

        int totalCards = columns * rows;

        // Ensure there is an even number of cards for matching pairs
        if (totalCards % 2 != 0)
        {
            Debug.LogError("The total number of cards must be an even number for matching pairs.");
            return;
        }

        // Clear any existing cards in the grid
        foreach (Transform child in gridParent)
        {
            Destroy(child.gameObject);
        }

        // Step 1: Duplicate the available sprites from LevelData to create pairs
        randomizedSprites = new List<Sprite>(availableSprites);
        randomizedSprites.AddRange(availableSprites); // Create pairs by duplicating the sprites

        // Step 2: Shuffle the randomized sprite list
        Shuffle(randomizedSprites);

        // Step 3: Instantiate cards and assign the randomized sprites
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
