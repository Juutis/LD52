using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "DifficultyConfig", menuName = "Configs/Difficulty Config")]
public class DifficultyScriptableObject : ScriptableObject
{
    [SerializeField]
    private List<Difficulty> difficulties;
    public Difficulty Get(DifficultyType difficultyType)
    {
        return difficulties.FirstOrDefault(difficulty => difficulty.Type == difficultyType);
    }

    public Difficulty CurrentDifficulty
    {
        get
        {
            DifficultyType difficultyType = (DifficultyType)PlayerPrefs.GetInt("difficulty", 1);
            return Get(difficultyType);
        }
    }

    public void SaveDifficulty(Difficulty difficulty)
    {
        PlayerPrefs.SetInt("difficulty", (int)difficulty.Type);
    }
}

public enum DifficultyType
{
    Easy,
    Normal,
    Hard,
    Speedrunner
}


[System.Serializable]
public class Difficulty
{
    [SerializeField]
    private DifficultyType type;
    public DifficultyType Type { get { return type; } }
    [SerializeField]
    private Color color;
    public Color Color { get { return color; } }

    [SerializeField]
    private int health;
    public int Health { get { return health; } }
}
