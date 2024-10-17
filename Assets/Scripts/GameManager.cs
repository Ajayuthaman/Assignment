using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public List<CardController> flippedCards = new List<CardController>();
    public float flipBackDelay = 1f;
    public float shrinkDuration = 0.5f;
    [HideInInspector] public bool isProcessing = false;
    public CanvasGroup canvasGroup;
    public Transform gridParent;
    [HideInInspector] public int clickCount = 0;

    public LevelManager levelManager;
    public GameObject menuPanel;
    public GameObject gamePanel;

    private void Start()
    {
        menuPanel.SetActive(true);
        gamePanel.SetActive(false);
    }

    public void OnCardFlipped(CardController flippedCard)
    {
        if (isProcessing) return; 

        // Add the flipped card to the list
        flippedCards.Add(flippedCard);

        // If two cards are flipped, comparing them
        if (flippedCards.Count == 2)
        {
            CompareCards();
        }
    }

    public void CheckClickable()
    {
        clickCount++;
        if (clickCount > 1)
        {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false; 
        }
    }

    // Compare two flipped cards
    private void CompareCards()
    {
        isProcessing = true; 

        CardController card1 = flippedCards[0];
        CardController card2 = flippedCards[1];

        // Check if the cards sprites are same
        if (card1.frontImage.sprite == card2.frontImage.sprite)
        {
            // Cards match
            card1.isMatched = true;
            card2.isMatched = true;

            // Shrink to size zero
            card1.ShrinkOnMatch();
            card2.ShrinkOnMatch();

            //play correct sound
            AudioManager.instance.PlayCorrectSound();

            // Clear the flipped cards list for the next pair
            flippedCards.Clear();

            // Check if all cards are matched
            if (AreAllCardsMatched())
            {
                AudioManager.instance.PlayWinSound();
                LoadNextLevel();
            }
        }
        else
        {
            // Cards don't match, fliping back after a short delay
            DOVirtual.DelayedCall(flipBackDelay, () =>
            {
                card1.FlipCardBack();
                card2.FlipCardBack();

                AudioManager.instance.PlayWrongSound();

                // Clear the flipped cards list for the next pair
                flippedCards.Clear();
            });
        }

        // Unlock processing and enable interaction again
        isProcessing = false;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        clickCount = 0;
    }

    // Check if all cards in the game are matched
    private bool AreAllCardsMatched()
    {
        CardController[] allCards = FindObjectsOfType<CardController>();

        foreach (CardController card in allCards)
        {
            if (!card.isMatched) return false;
        }
        return true;
    }

    // Load the next level using LevelManager
    private void LoadNextLevel()
    {
        levelManager.LoadNextLevel();
    }


    //Menu button functions
    public void PlayBtn()
    {
        levelManager.RestartGame();
        gamePanel.SetActive(true);
        menuPanel.SetActive(false);

        AudioManager.instance.PlayBackgroundMusic();
    }

    public void LoadBtn()
    {
        levelManager.LoadCurrentLevel();
        gamePanel.SetActive(true);
        menuPanel.SetActive(false);

        AudioManager.instance.PlayBackgroundMusic();
    }

    public void QuitBtn()
    {
        Application.Quit();
    }

    //Game panel

    public void HomeBtn()
    {
        // Clear any existing cards in the grid
        foreach (Transform child in gridParent)
        {
            Destroy(child.gameObject);
        }
        gamePanel.SetActive(false);
        menuPanel.SetActive(true);
        AudioManager.instance.StopAllSound();
    }

}
