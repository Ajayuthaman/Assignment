using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public LevelData[] levels;
    private int currentLevelIndex = 0;

    public CardLayoutManager cardLayoutManager;
    public DynamicGridScaler gridScaler;

    private const string CurrentLevelKey = "CurrentLevelIndex"; // Key for saving the level index

/*    private void Start()
    {
        LoadCurrentLevel();
    }*/

    // Method to load the current level based on saved index
    public void LoadCurrentLevel()
    {
        // Load the saved level index when starting the game
        currentLevelIndex = PlayerPrefs.GetInt(CurrentLevelKey, 0); // Default to level 0 if not saved

        LoadLevelByIndex(currentLevelIndex);
    }

    // Method to load the next level
    public void LoadNextLevel()
    {
        currentLevelIndex++;
        if (currentLevelIndex < levels.Length)
        {
            SaveCurrentLevel(); // Save the current level index before loading the next one
            LoadLevelByIndex(currentLevelIndex);
        }
        else
        {
            Debug.Log("All levels completed!");
        }
    }

    // Restart the game 
    public void RestartGame()
    {
        currentLevelIndex = 0;
        SaveCurrentLevel(); // Save the current level index when restarting
        LoadLevelByIndex(currentLevelIndex);
    }

    // Method to save the current level index
    private void SaveCurrentLevel()
    {
        PlayerPrefs.SetInt(CurrentLevelKey, currentLevelIndex);
        PlayerPrefs.Save(); 
        Debug.Log($"Current Level Saved: {currentLevelIndex}");
    }

    // Method to start a new level (given a specific level index)
    public void StartNewLevel(int levelIndex)
    {
        if (levelIndex >= 0 && levelIndex < levels.Length)
        {
            currentLevelIndex = levelIndex;
            SaveCurrentLevel(); // Save the new level index
            LoadLevelByIndex(currentLevelIndex); // Load the new level
        }
        else
        {
            Debug.LogError("Invalid level index. Cannot start new level.");
        }
    }

    // Method to load a specific level by index
    public void LoadLevelByIndex(int levelIndex)
    {
        if (levelIndex >= 0 && levelIndex < levels.Length)
        {
            currentLevelIndex = levelIndex;
            LevelData levelData = levels[currentLevelIndex];
            gridScaler.AdjustGridLayout(levelData.columns, levelData.rows);
            cardLayoutManager.SetUpLevel(levelData);
            Debug.Log($"Loaded Level {levelData.levelNumber}");
        }
        else
        {
            Debug.LogError("Invalid level index. Cannot load level.");
        }
    }
}
