using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager main;

    [SerializeField]
    private DifficultyScriptableObject difficultyConfig;
    // Start is called before the first frame update
    public Difficulty Difficulty { get { return difficultyConfig.CurrentDifficulty; } }

    private GameTimer gameTimer;
    public GameTimer GameTimer { get { return gameTimer; } }
    private bool paused = false;
    public bool Paused { get { return paused; } }

    [SerializeField]
    private int maxBerries = 100;
    private int currentBerryCount = 0;
    private void Awake()
    {
        main = this;
    }

    void Start()
    {
        gameTimer = new GameTimer();
        UIManager.main.Initialize(gameTimer, Difficulty, maxBerries);
    }

    private void Pause()
    {
        Time.timeScale = 0f;
        gameTimer.Pause();
        GameRhythm.main.Pause();
        paused = true;
    }
    private void Unpause()
    {
        Time.timeScale = 1f;
        gameTimer.Unpause();
        GameRhythm.main.Unpause();
        paused = false;
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            AddBerry(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            AddBerry(6);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            AddBerry(4);
        }
    }
    public void AddBerry(int count)
    {
        currentBerryCount += count;
        UIManager.main.AddBerry(count);
    }

    public void PauseGame()
    {
        Pause();
    }

    public void ResumeGame()
    {
        Unpause();
    }
    public void OpenMainMenu()
    {
        Unpause();
        SceneManager.LoadScene(0);
    }
    public void RestartGame()
    {
        Unpause();
        SceneManager.LoadScene(1);
    }

    public void GameOver()
    {
        UIManager.main.OpenGameOverMenu();
    }
    public void TheEnd()
    {
        UIManager.main.OpenTheEndMenu();
    }


}
