using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private int levelIndex;
    public Level[] levelPrefabs;
    public CrossBowController crossBowController;
    public Level currentLevel;

    private void Awake()
    {
        //levelIndex = PlayerPrefs.GetInt("Level", 1);
    }

    private void Start()
    {
        LoadLevel(levelIndex - 1);
        OnInit();
        UIManager.Instance.OpenUI<MainMenu>();
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

    public void LoadLevel(int level)
    {
        if(currentLevel != null)
        {
            Destroy(currentLevel.gameObject);
        }

        if (level < levelPrefabs.Length)
        {
            currentLevel = Instantiate(levelPrefabs[level]);
            currentLevel.OnInit();
        }

        else if(level >= levelPrefabs.Length)
        {
            //Debug.Log("All levels completed!");
            //GameManager.Instance.ChangeState(GameState.MainMenu);
            //return;
            levelIndex = 0;
            currentLevel = Instantiate(levelPrefabs[0]);
        }    

    }

    public void OnStartGame()
    {
        GameManager.Instance.ChangeState(GameState.GamePlay);


        //
        //OnInit();
    }

    public void OnPause()
    {
        crossBowController.hasShot = true;
        GameManager.Instance.ChangeState(GameState.Pause);
    }

    public void OnContinue()
    {
        crossBowController.hasShot = false;
        GameManager.Instance.ChangeState(GameState.GamePlay);
    }

    public void OnReset()
    {
        SimplePool.CollectAll();
    }

    public void OnRetry()
    {
        OnReset();
        LoadLevel(levelIndex - 1);
        OnInit();
        UIManager.Instance.OpenUI<GamePlay>();
        //OnInit();
        //UIManager.Instance.OpenUI<MainMenu>();
    }    


    public void OnNextLevel()
    {
        levelIndex++;
        //PlayerPrefs.SetInt("Level", levelIndex);
        //OnReset();
        ////LoadLevel();
        OnReset();
        LoadLevel(levelIndex - 1);
        OnInit();
        UIManager.Instance.OpenUI<GamePlay>();
    }
}
