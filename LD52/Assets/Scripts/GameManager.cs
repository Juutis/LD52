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

    [SerializeField]
    private AudioSource MenuMusic;
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
        MenuMusic.Play();
        paused = true;
    }
    private void Unpause()
    {
        Time.timeScale = 1f;
        gameTimer.Unpause();
        MenuMusic.Pause();
        GameRhythm.main.Unpause();
        paused = false;
    }


    public void AddBerry(int count)
    {
        currentBerryCount += count;
        UIManager.main.AddBerry(count);
        SoundManager.main.PlaySound(GameSoundType.EatDaBerry);
    }

    public void PauseGame()
    {
        Pause();
    }

    public void ResumeGame()
    {
        Unpause();
    }

    public void RespawnAtCheckpoint()
    {
        PlayerMovement.main.transform.position = new Vector3(
            CheckpointManager.main.MostRecentCheckpoint.SpawnPoint.x,
            PlayerMovement.main.transform.position.y,
            CheckpointManager.main.MostRecentCheckpoint.SpawnPoint.z
        );
        EntityHealth.player.Initialize();
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
        LevelManager.main.Restart();
    }

    public void GameOver()
    {
        UIManager.main.OpenGameOverMenu();
    }
    public void TheEnd()
    {
        UIManager.main.OpenTheEndMenu();
    }
    public void NextLevelScreen()
    {
        if (LevelManager.main.IsLastLevel())
        {
            TheEnd();
        }
        else
        {
            UIManager.main.OpenNextLevelMenu();
        }

    }

}
