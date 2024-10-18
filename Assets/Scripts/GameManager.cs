using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using System.Collections;

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

    //win particle
    public ParticleSystem winParticle;

    // Level text for tracking matched cards and attempts
    public TMP_Text matchedCardNo;
    public TMP_Text totalAttemptsNo;
    public TMP_Text scoreTxt;
    public TMP_Text titleTxt;
    public TMP_Text highScoreTxt;

    private int highScore = 0;
    private int totalAttempts;
    private int matchedCardsCount = 0;
    private int scoreNo = 0;
    private int combo = 0;

    //next level panel buttons
    public GameObject nextLevelPanel;
    public RectTransform[] buttons; 
    public float dropPositionY = 0f; 
    public float dropDuration = 1f; 
    public float bounceHeight = 100f; 
    public float bounceDuration = 0.5f; 
    private Vector3[] initialPositions;

    private bool isWin;

    private void Start()
    {
        menuPanel.SetActive(true);
        gamePanel.SetActive(false);
        nextLevelPanel.SetActive(false);

        matchedCardsCount = 0;

        // Store the initial positions of the buttons
        initialPositions = new Vector3[buttons.Length];
        for (int i = 0; i < buttons.Length; i++)
        {
            initialPositions[i] = buttons[i].localPosition;
        }

        highScore = PlayerPrefs.GetInt("HighScore", 0);
        UpdateHighScoreUI();
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

            combo++;
            matchedCardsCount += 1;

            scoreNo += (combo > 2) ? 10 : 5;


            UpdateUI();  

            // Clear the flipped cards list for the next pair
            flippedCards.Clear();

            // Check if all cards are matched
            if (AreAllCardsMatched())
            {
                AudioManager.instance.PlayWinSound();
                winParticle.Play();
                isWin = true;
                combo = 0;

                // Check and update high score
                if (scoreNo > highScore)
                {
                    highScore = scoreNo;
                    PlayerPrefs.SetInt("HighScore", highScore); // Save high score
                    UpdateHighScoreUI();
                }

                Invoke(nameof(AnimateButtons), 3f);
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
                isWin = false;
                // Clear the flipped cards list for the next pair
                flippedCards.Clear();

                totalAttempts--;
                combo = 0;
                UpdateUI(); 

                if(totalAttempts == 0)
                {
                    AudioManager.instance.PlayFailSound();
                    AnimateButtons();
                }
            });
        }

        // Unlock processing and enable interaction again
        isProcessing = false;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        clickCount = 0;
    }

    public void UpdateGameScore(int _totalAttempts)
    {
        totalAttempts = _totalAttempts;
        matchedCardsCount = 0;
        UpdateUI();
    }

    private void UpdateUI()
    {
        matchedCardNo.text = matchedCardsCount.ToString();
        totalAttemptsNo.text = totalAttempts.ToString();
        scoreTxt.text = scoreNo.ToString();
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
        totalAttempts = 0;
        matchedCardsCount = 0;
        levelManager.LoadNextLevel();
    }

    // Menu button functions
    public void PlayBtn()
    {
        totalAttempts = 0;
        matchedCardsCount = 0;
        scoreNo = 0;

        levelManager.RestartGame();
        gamePanel.SetActive(true);
        menuPanel.SetActive(false);

        AudioManager.instance.PlayBackgroundMusic();
    }

    public void LoadBtn()
    {
        totalAttempts = 0;
        matchedCardsCount = 0;

        levelManager.LoadCurrentLevel();
        gamePanel.SetActive(true);
        menuPanel.SetActive(false);
        nextLevelPanel.SetActive(false);

        AudioManager.instance.PlayBackgroundMusic();
    }

    public void NextBtn()
    {
        totalAttempts = 0;
        matchedCardsCount = 0;

        levelManager.LoadNextLevel();
        gamePanel.SetActive(true);
        menuPanel.SetActive(false);
        nextLevelPanel.SetActive(false);

    }

    public void RetryBtn()
    {
        totalAttempts = 0;
        matchedCardsCount = 0;
        scoreNo = 0;
        UpdateUI();

        levelManager.LoadCurrentLevel();
        gamePanel.SetActive(true);
        menuPanel.SetActive(false);
        nextLevelPanel.SetActive(false);

        AudioManager.instance.PlayBackgroundMusic();
    }

    public void QuitBtn()
    {
        Application.Quit();
    }

    // Game panel

    public void HomeBtn()
    {
        // Clear any existing cards in the grid
        foreach (Transform child in gridParent)
        {
            Destroy(child.gameObject);
        }
        gamePanel.SetActive(false);
        nextLevelPanel.SetActive(false);
        menuPanel.SetActive(true);
        AudioManager.instance.StopAllSound();
    }

    //next level panel buttons
    void AnimateButtons()
    {
        ResetButtons();
        nextLevelPanel.SetActive(true);

        buttons[1].gameObject.SetActive(isWin && !levelManager.allLevelsCompleted);

        if(isWin && !levelManager.allLevelsCompleted)
        {
            titleTxt.text = "WIN";
        }
        else if (levelManager.allLevelsCompleted && isWin)
        {
            titleTxt.text = "Levels Completed";
        }
        else
        {
            titleTxt.text = "Failed";
        }


        for (int i = 0; i < buttons.Length; i++)
        {
            RectTransform button = buttons[i];
            float delay = i * 0.2f; // Adding a small delay between each button's animation

            // Move the button down to the bounce point
            button.DOLocalMoveY(dropPositionY, dropDuration)
                .SetEase(Ease.Linear)
                .SetDelay(delay)
                .OnComplete(() => {
                    // Bounce effect
                    button.DOLocalMoveY(dropPositionY - bounceHeight, bounceDuration)
                          .SetEase(Ease.OutQuad)
                          .SetLoops(2, LoopType.Yoyo);
                });
        }

        // Animate the TMP_Text object
        float textDelay = buttons.Length * 0.2f; // Delay the text animation after the last button

        // Ensure the text starts fully transparent
        titleTxt.alpha = 0f;

        // Fade in the TMP_Text object
        titleTxt.DOFade(1f, 0.5f)
            .SetEase(Ease.Linear)
            .SetDelay(textDelay);
    }

    void ResetButtons()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].localPosition = initialPositions[i];
        }
    }

    public IEnumerator AllLevelComplet()
    {
        winParticle.Play();
        yield return new WaitForSeconds(3f);
        gamePanel.SetActive(false);
        nextLevelPanel.SetActive(false);
        menuPanel.SetActive(true);
    }

    private void UpdateHighScoreUI()
    {
        highScoreTxt.text = highScore.ToString();
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.Save();
    }

}
