using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIGameTimer : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI txtTimer;
    private GameTimer gameTimer;

    public void Initialize(GameTimer timer)
    {
        gameTimer = timer;
    }

    void Update()
    {
        if (gameTimer != null)
        {
            txtTimer.text = gameTimer.GetString();
        }
    }
}
