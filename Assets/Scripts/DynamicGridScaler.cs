using UnityEngine;
using UnityEngine.UI;

public class DynamicGridScaler : MonoBehaviour
{
    public GridLayoutGroup gridLayoutGroup; 
    public int columns = 3; 
    public int rows = 3;    
    public float maxCardSize = 100f; 
    public float paddingPercentage = 0.05f; 

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

        // Calculating the ideal card size based on available space and number of rows/columns
        float idealCardWidth = (availableWidth - (paddingX * 2)) / columns;
        float idealCardHeight = (availableHeight - (paddingY * 2)) / rows;

        // Start with the smaller dimension to ensure equal width and height, but limit to max size
        float cardSize = Mathf.Min(Mathf.Min(idealCardWidth, idealCardHeight), maxCardSize);

        // Calculatinf spacing based on the remaining space
        float totalWidthWithSpacing = (cardSize * columns);
        float totalHeightWithSpacing = (cardSize * rows);

        // Calculating the available spacing
        float spacingX = (availableWidth - totalWidthWithSpacing) / (columns - 1);
        float spacingY = (availableHeight - totalHeightWithSpacing) / (rows - 1);

        // Ensure spacingg is not negative
        spacingX = Mathf.Max(spacingX, 0);
        spacingY = Mathf.Max(spacingY, 0);

        // Set the cell size to ensure square cards
        gridLayoutGroup.cellSize = new Vector2(cardSize, cardSize);

        gridLayoutGroup.spacing = new Vector2(spacingX, spacingY);

        LayoutRebuilder.ForceRebuildLayoutImmediate(gridRectTransform);

        Debug.Log($"Desired Columns: {columns}, Rows: {rows}");
        Debug.Log($"Available Width: {availableWidth}, Height: {availableHeight}");
        Debug.Log($"Card Size: {cardSize}");
        Debug.Log($"Calculated Spacing X: {spacingX}, Spacing Y: {spacingY}");
    }
}
