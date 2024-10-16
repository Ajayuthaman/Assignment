using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using DG.Tweening.Core.Easing;

public class CardController : MonoBehaviour
{
    public Image frontImage;
    public Image backImage;
    public bool isMatched = false;
    private bool isFlipped = false;
    public float flipDuration = 0.5f;

    public void FlipCard()
    {
        if (isMatched || isFlipped) return; // If the card is already matched or flipped, don't flip it again

        // Animate the rotation along the Y-axis to flip
        transform.DORotate(new Vector3(0f, 90f, 0f), flipDuration / 2).OnComplete(() =>
        {
            frontImage.gameObject.SetActive(true);
            backImage.gameObject.SetActive(false);

            transform.DORotate(new Vector3(0f, 0f, 0f), flipDuration / 2);

        });

        isFlipped = true;
    }

    // Flip the card back to show the back image (if cards don't match)
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
}
