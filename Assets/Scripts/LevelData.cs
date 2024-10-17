using UnityEngine;

[CreateAssetMenu(fileName = "Levels", menuName = "LevelData")]
public class LevelData : ScriptableObject
{
    public int rows;
    public int columns;
    public Sprite[] levelImages;
    public int levelNumber;
}
