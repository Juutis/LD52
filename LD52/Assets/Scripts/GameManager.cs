using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager main;

    [SerializeField]
    private DifficultyScriptableObject difficultyConfig;
    // Start is called before the first frame update
    public Difficulty Difficulty { get { return difficultyConfig.CurrentDifficulty; } }

    private GameTimer gameTimer;
    private void Awake()
    {
        main = this;
    }

    void Start()
    {
        gameTimer = new GameTimer();
        UIManager.main.Initialize(gameTimer, Difficulty);
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        gameTimer.Pause();
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        gameTimer.Unpause();
    }


}
