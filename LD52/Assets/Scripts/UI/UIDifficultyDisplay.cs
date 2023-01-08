using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIDifficultyDisplay : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI txtDifficulty;
    public void Initialize(Difficulty difficulty)
    {
        txtDifficulty.text = $"Difficulty: {difficulty.Type}";
    }
}
