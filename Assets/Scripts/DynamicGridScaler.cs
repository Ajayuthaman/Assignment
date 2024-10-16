using UnityEngine;
using UnityEngine.UI;

public class DynamicGridScaler : MonoBehaviour
{
    public GridLayoutGroup gridLayoutGroup; 
    public int columns = 3; 
    public int rows = 3; 

    public float cardAspectRatio = 1f; 
    public float paddingPercentage = 0.05f; 
    public float minSpacingY = 10f;

    void Start()
    {
        AdjustGridLayout();
    }

    void AdjustGridLayout()
    {
        RectTransform gridRectTransform = GetComponent<RectTransform>();
        float screenWidth = gridRectTransform.rect.width;
        float screenHeight = gridRectTransform.rect.height;

        // Calculatinf padding based on the screen size
        float paddingX = screenWidth * paddingPercentage;
        float paddingY = screenHeight * paddingPercentage;

        // Calculating the available width and height for the grid after padding
        float availableWidth = screenWidth - paddingX;
        float availableHeight = screenHeight - paddingY;

        // Calculating card size based on available space and number of rows/columns
        float cardWidth = availableWidth / columns;
        float cardHeight = availableHeight / rows;

        // Adjust card height based on the aspect ratio
        if (cardAspectRatio > 1)
        {
            cardHeight = cardWidth / cardAspectRatio; // For wider cards
        }
        else
        {
            cardWidth = cardHeight * cardAspectRatio; // For taller cards
        }

        gridLayoutGroup.cellSize = new Vector2(cardWidth, cardHeight);

        // Adjust the spacing dynamically based on the card size
        float spacingX = (availableWidth - (cardWidth * columns)) / (columns - 1);
        float spacingY = (availableHeight - (cardHeight * rows)) / (rows - 1);

        // Ensure there is always a minimum vertical spacing between cards
        if (spacingY < minSpacingY)
        {
            spacingY = minSpacingY; // Set a minimum vertical spacing if calculated value is too small
        }

        gridLayoutGroup.spacing = new Vector2(spacingX, spacingY);

        Debug.Log($"Card Width: {cardWidth}, Card Height: {cardHeight}");
        Debug.Log($"Spacing X: {spacingX}, Spacing Y: {spacingY}");
    }
}
