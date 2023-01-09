using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private List<ScenePicker> scenes;

    public static LevelManager main;
    void Awake()
    {
        if (GameObject.FindGameObjectsWithTag("LevelManager").Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(this);
            main = this;
        }
    }

    int nextLevelIndex = 0;
    int currentLevelIndex = 0;
    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        nextLevelIndex = 0;
        Debug.Log("Start levelmanager");
    }

    public void Restart()
    {
        string sceneToLoad = scenes[currentLevelIndex].scenePath;
        SceneManager.LoadScene(sceneToLoad);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scenewasloaded: " + scene.name);
    }

    public void StartGame()
    {
        nextLevelIndex = 0;
        NextLevel();
    }

    public void NextLevel()
    {
        if (IsLastLevel())
        {
            Debug.Log("Cant load next level!");
            return;
        }
        currentLevelIndex = nextLevelIndex;
        string sceneToLoad = scenes[nextLevelIndex].scenePath;
        nextLevelIndex += 1;
        SceneManager.LoadScene(sceneToLoad);
        Time.timeScale = 1f;
    }

    public bool IsLastLevel()
    {
        return nextLevelIndex >= scenes.Count;
    }

    void Update()
    {
        if (Application.isEditor && Input.GetKeyDown(KeyCode.P))
        {
            NextLevel();
        }
    }
}
