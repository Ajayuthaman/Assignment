using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CardController : MonoBehaviour
{
    public Image frontImage;   
    public Image backImage;    
    public bool isMatched = false; 

    private bool isFlipped = false; 
    private GameManager gameManager; 
    public float flipDuration = 0.5f;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        // Show the front image for 1 second at the start of the game
        ShowFrontTemporarily();
    }

    public void SetFrontImage(Sprite sprite)
    {
        frontImage.sprite = sprite;
        frontImage.SetNativeSize(); 
    }

    // Show the front image for 1 second at the start of the game
    public void ShowFrontTemporarily()
    {
        // Flip to front immediately
        frontImage.gameObject.SetActive(true);
        backImage.gameObject.SetActive(false);

        // After 1 second, flip back to show the back image
        DOVirtual.DelayedCall(1f, () =>
        {
            FlipCardBack();
        });
    }

    // This method will flip the card from back to front
    public void FlipCard()
    {
        if (isMatched || isFlipped) return; // If the card is already matched or flipped, pause the flipping

        // Animate the rotation along the Y-axis to flip
        transform.DORotate(new Vector3(0f, 90f, 0f), flipDuration / 2).OnComplete(() =>
        {
            frontImage.gameObject.SetActive(true);
            backImage.gameObject.SetActive(false);

            transform.DORotate(new Vector3(0f, 0f, 0f), flipDuration / 2).OnComplete(() =>
            {
                gameManager.OnCardFlipped(this);
            });
        });

        isFlipped = true;
    }

    // Flip the card back to show the back image (used if cards don't match)
    public void FlipCardBack()
    {
        transform.DORotate(new Vector3(0f, 90f, 0f), flipDuration / 2).OnComplete(() =>
        {
            frontImage.gameObject.SetActive(false);
            backImage.gameObject.SetActive(true);

            transform.DORotate(new Vector3(0f, 0f, 0f), flipDuration / 2);
        });

        isFlipped = false;
    }

    // Method to shrink the card to zero when matched
    public void ShrinkOnMatch()
    {
        // Shrink the card to 0 scale
        transform.DOScale(Vector3.zero, 0.5f).OnComplete(() =>
        {
            //gameObject.SetActive(false); 
        });
    }
}
