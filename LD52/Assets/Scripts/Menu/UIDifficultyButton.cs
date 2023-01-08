using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIDifficultyButton : MonoBehaviour
{
    private int difficultyIndex;

    [SerializeField]
    private List<Image> images;

    [SerializeField]
    private TextMeshProUGUI txt;

    [SerializeField]
    private DifficultyScriptableObject difficultyConfig;

    private int max = System.Enum.GetValues(typeof(DifficultyType)).GetUpperBound(0);

    void Start()
    {
        Difficulty difficulty = difficultyConfig.CurrentDifficulty;
        difficultyIndex = (int)difficulty.Type;
        DisplayDifficulty(difficulty);
    }

    public void NextDifficulty()
    {
        difficultyIndex += 1;
        if (difficultyIndex > max)
        {
            difficultyIndex = 0;
        }
        Difficulty difficulty = difficultyConfig.Get((DifficultyType)difficultyIndex);
        ChangeDifficulty(difficulty);
    }

    private void DisplayDifficulty(Difficulty difficulty)
    {
        images.ForEach(image => image.color = difficulty.Color);
        txt.text = $"Difficulty: {difficulty.Type}";
    }

    private void ChangeDifficulty(Difficulty difficulty)
    {
        DisplayDifficulty(difficulty);
        difficultyConfig.SaveDifficulty(difficulty);
    }

}
