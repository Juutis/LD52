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

    int levelIndex = 0;
    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        levelIndex = 0;
        Debug.Log("Start levelmanager");
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scenewasloaded: " + scene.name);
    }

    public void StartGame()
    {
        levelIndex = 0;
        NextLevel();
    }

    public void NextLevel()
    {

        if (IsLastLevel())
        {
            Debug.Log("Cant load next level!");
            return;
        }
        string sceneToLoad = scenes[levelIndex].scenePath;
        levelIndex += 1;
        SceneManager.LoadScene(sceneToLoad);
    }

    public bool IsLastLevel()
    {
        return levelIndex >= scenes.Count;
    }

    void Update()
    {
        if (Application.isEditor && Input.GetKeyDown(KeyCode.P))
        {
            NextLevel();
        }
    }
}
