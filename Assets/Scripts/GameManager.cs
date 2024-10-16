using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public List<CardController> flippedCards = new List<CardController>(); 
    public float flipBackDelay = 1f; 

    
    public void OnCardFlipped(CardController flippedCard)
    {
        // Add the flipped card to the list
        flippedCards.Add(flippedCard);

        // comparing cards
        if (flippedCards.Count == 2)
        {
            CompareCards();
        }
    }

    private void CompareCards()
    {
        CardController card1 = flippedCards[0];
        CardController card2 = flippedCards[1];

        // Check if the cards match by the sprite
        if (card1.frontImage.sprite == card2.frontImage.sprite)
        {
            // Cards match, keep them flipped
            card1.isMatched = true;
            card2.isMatched = true;

            // Animate shrinking the cards to 0 when matched
            card1.ShrinkOnMatch();
            card2.ShrinkOnMatch();

            // Clear the flipped cards list for the next pair
            flippedCards.Clear();
        }
        else
        {
            // Cards don't match, flip them back after a short delay
            DOVirtual.DelayedCall(flipBackDelay, () =>
            {
                card1.FlipCardBack();                                                                                                                                                                                   
                card2.FlipCardBack();

                // Clear the flipped cards list for the next pair
                flippedCards.Clear();
            });
        }
    }
}
