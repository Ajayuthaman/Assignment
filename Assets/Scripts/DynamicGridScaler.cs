using UnityEngine;
using UnityEngine.UI;

public class DynamicGridScaler : MonoBehaviour
{
    public GridLayoutGroup gridLayoutGroup;
    public float maxCardSize = 100f;
    public float desiredSpacing = 10f;
    public float paddingPercentage = 0.05f;


    public void AdjustGridLayout(int columns, int rows)
    {
        
        RectTransform gridRectTransform = GetComponent<RectTransform>();
        float screenWidth = gridRectTransform.rect.width;
        float screenHeight = gridRectTransform.rect.height;

        // Calculatif padding based on the screen size
        float paddingX = screenWidth * paddingPercentage;
        float paddingY = screenHeight * paddingPercentage;

        // Calculating the available width and height for the grid after padding
        float availableWidth = screenWidth - paddingX;
        float availableHeight = screenHeight - paddingY;

        // Calculating the ideal card size based on available space and number of rows/columns
        float cardWidth = (availableWidth - (desiredSpacing * (columns - 1))) / columns;
        float cardHeight = (availableHeight - (desiredSpacing * (rows - 1))) / rows;

        // Using the smaller dimension to ensure equal width and height, but also limit to max size
        float cardSize = Mathf.Min(Mathf.Min(cardWidth, cardHeight), maxCardSize);

        // Set the cell size to ensure square cards
        gridLayoutGroup.cellSize = new Vector2(cardSize, cardSize);

        // Calculating spacing dynamically with conditions
        float spacingX = columns > 1 ? (availableWidth - (cardSize * columns)) / (columns - 1) : 0;
        float spacingY = rows > 1 ? (availableHeight - (cardSize * rows)) / (rows - 1) : 0;

        // Limit the minimum spacing to a reasonable value, ensuring it does not exceed desiredSpacing
        spacingX = Mathf.Max(spacingX, desiredSpacing);
        spacingY = Mathf.Max(spacingY, desiredSpacing * 1.5f);

        // Prevent spacing from becoming excessively large
        if (spacingX > (availableWidth / columns) - cardSize) spacingX = (availableWidth / columns) - cardSize;
        if (spacingY > (availableHeight / rows) - cardSize) spacingY = (availableHeight / rows) - cardSize;

        // Set the calculated spacing
        gridLayoutGroup.spacing = new Vector2(spacingX, spacingY);

        // Force the layout to update
        LayoutRebuilder.ForceRebuildLayoutImmediate(gridRectTransform);

        Debug.Log($"Desired Columns: {columns}, Rows: {rows}");
        Debug.Log($"Available Width: {availableWidth}, Height: {availableHeight}");
        Debug.Log($"Card Size: {cardSize}");
        Debug.Log($"Spacing X: {gridLayoutGroup.spacing.x}, Spacing Y: {gridLayoutGroup.spacing.y}");
    }
}
