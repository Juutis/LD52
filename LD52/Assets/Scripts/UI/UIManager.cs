using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager main;

    [SerializeField]
    private UIGameTimer uiGameTimer;
    [SerializeField]
    private UIDifficultyDisplay uiDifficultyDisplay;
    private void Awake()
    {
        main = this;
    }

    public void Initialize(GameTimer timer, Difficulty difficulty)
    {
        uiGameTimer.Initialize(timer);
        uiDifficultyDisplay.Initialize(difficulty);
    }

    [SerializeField]
    private UIHealth uiPlayerHealth;

    public void RegisterPlayerHealth(EntityHealth playerHealth)
    {
        uiPlayerHealth.Initialize(playerHealth);
    }

}
