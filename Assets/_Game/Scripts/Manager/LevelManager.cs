using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private int levelIndex;
    public List<Level> levelPrefabs = new List<Level>();
    public CrossBowController crossBowController;
    public Level currentLevel;

    private void Awake()
    {
        //levelIndex = PlayerPrefs.GetInt("Level", 1);
    }

    private void Start()
    {
        LoadLevel();
    }
    public void OnInit()
    {
        crossBowController.OnInit();
        //CrossBowController crossBowController = GetComponent<CrossBowController>();
        if (currentLevel != null && currentLevel.crossbowPosition != null)
        {
            //crossbowTransform.position = level.crossbowPosition.position;
            //crossbowTransform.rotation = level.crossbowPosition.rotation;
            crossBowController.transform.position = currentLevel.crossbowPosition.position;
            crossBowController.transform.rotation = currentLevel.crossbowPosition.rotation;
        }
    }

    public void LoadLevel()
    {
        LoadLevel(levelIndex - 1);
        OnInit();
    }

    public void LoadLevel(int indexLevel)
    {
        if(currentLevel != null)
        {
            Destroy(currentLevel.gameObject);
        }

        if (levelIndex <= levelPrefabs.Count)
        {
            currentLevel = Instantiate(levelPrefabs[indexLevel]);
        }

        else if(levelIndex > levelPrefabs.Count)
        {
            Debug.Log("All levels completed!");
            GameManager.Instance.ChangeState(GameState.MainMenu);
            return;
        }    

    }

    public void OnStartGame()
    {
        GameManager.Instance.ChangeState(GameState.GamePlay);
    }

    public void OnPause()
    {
        GameManager.Instance.ChangeState(GameState.Pause);
    }

    public void OnRetry()
    {
        OnReset();
        LoadLevel();

        GameManager.Instance.ChangeState(GameState.MainMenu);
    }    

    public void OnReset()
    {
        SimplePool.CollectAll();
    }

    public void OnNextLevel()
    {
        levelIndex++;
        PlayerPrefs.SetInt("Level", levelIndex);
        OnReset();
        LoadLevel();
    }
}
