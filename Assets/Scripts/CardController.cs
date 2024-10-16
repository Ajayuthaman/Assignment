using UnityEngine;
using UnityEngine.UI;

public class CardController : MonoBehaviour
{
    public Image frontImage;  
    public Image backImage;   

    private bool isFlipped = false; 

    public void FlipCard()
    {

        if (!isFlipped)
        {
            // Show the front image and hide the back
            frontImage.gameObject.SetActive(true);
            backImage.gameObject.SetActive(false);
        }
        else
        {
            // Show the back image and hide the front
            frontImage.gameObject.SetActive(false);
            backImage.gameObject.SetActive(true);
        }

        isFlipped = !isFlipped;
    }
}
