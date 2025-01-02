using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private CrossBowController crossBowPrefab;

    private CrossBowController crossBowInstance;
    public int levelIndex;
    public Level[] levelPrefabs;
    //public CrossBowController crossBowController;
    public Level currentLevel;

    private void Start()
    {
        levelIndex = PlayerPrefs.GetInt("Level", 1);
        UIManager.Instance.OpenUI<MainMenu>().ChangeAnim("fadeIn");
    }

    public void OnInit()
    {
        if(crossBowInstance == null)
        {
            crossBowInstance = Instantiate(crossBowPrefab);
        }

        crossBowInstance.OnInit();
        //crossBowController.OnInit();

        if (currentLevel != null && currentLevel.crossbowPosition != null)
        {
            //crossBowController.transform.SetPositionAndRotation(currentLevel.crossbowPosition.position, currentLevel.crossbowPosition.rotation);
            crossBowInstance.transform.SetPositionAndRotation(currentLevel.crossbowPosition.position, currentLevel.crossbowPosition.rotation);
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
            levelIndex = 1;
            //currentLevel = Instantiate(levelPrefabs[0]);
            UIManager.Instance.CloseAll();
            UIManager.Instance.OpenUI<MainMenu>().ChangeAnim("fadeIn");
        }
    }

    public void OnStartGame()
    {
        LoadLevel(levelIndex - 1);
        GameManager.Instance.ChangeState(GameState.GamePlay);
        OnInit();
    }

    public void OnPause()
    {
        GameManager.Instance.ChangeState(GameState.Pause);
    }

    public void OnContinue()
    {
        crossBowInstance.ResetState();
        GameManager.Instance.ChangeState(GameState.GamePlay);
    }

    public void OnReset()
    {
        SimplePool.CollectAll();
    }

    public void OnRetry()
    {
        OnReset();
        UIManager.Instance.OpenUI<GamePlay>();
    }    


    public void OnNextLevel()
    {
        levelIndex++;
        PlayerPrefs.SetInt("Level", levelIndex);
        //OnReset();
        ////LoadLevel();
        OnReset();
        UIManager.Instance.OpenUI<GamePlay>();
    }
}
//todo: fix playerprefs

