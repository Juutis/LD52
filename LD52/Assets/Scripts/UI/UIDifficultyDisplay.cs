using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIDifficultyDisplay : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI txtDifficulty;
    [SerializeField]
    private Image imgDifficulty;
    public void Initialize(Difficulty difficulty)
    {
        txtDifficulty.text = $"Difficulty: {difficulty.Type}";
        imgDifficulty.color = difficulty.Color;
    }
}
