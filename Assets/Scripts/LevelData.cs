using UnityEngine;

[CreateAssetMenu(fileName = "Levels", menuName = "LevelData")]
public class LevelData : ScriptableObject
{
    public int rows;
    public int columns;
    public int totalAttempt;
    public Sprite[] levelImages;
    public int levelNumber;
}
