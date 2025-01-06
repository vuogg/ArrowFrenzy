using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private CrossBowController crossBowPrefab;

    private CrossBowController crossBowInstance;

    public Level[] levelPrefabs;
    public Level currentLevel;
    public int levelIndex;

    private void Awake()
    {
        levelIndex = PlayerPrefs.GetInt(Constants.TAG_LEVEL, 1);

        if (levelIndex > 10)
        {
            levelIndex = 1;
            PlayerPrefs.SetInt(Constants.TAG_LEVEL, levelIndex);
            PlayerPrefs.Save(); // Lưu thay đổi vào PlayerPrefs
            UIManager.Instance.OpenUI<MainMenu>().ChangeAnim(Constants.ANIM_FADEIN);
        }
        else
        {
            UIManager.Instance.OpenUI<MainMenu>().ChangeAnim(Constants.ANIM_FADEIN);
        }
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

        //level = Mathf.Clamp(level, 0, levelIndex);

        if (level < levelPrefabs.Length)
        {
            currentLevel = Instantiate(levelPrefabs[level]);
            currentLevel.OnInit();
        }

        else if(level >= levelPrefabs.Length)
        {
            //levelIndex = 1;
            //PlayerPrefs.SetInt("Level", 1);
            //currentLevel = Instantiate(levelPrefabs[0]);
            UIManager.Instance.CloseAll();
            UIManager.Instance.OpenUI<MainMenu>().ChangeAnim(Constants.ANIM_FADEIN);
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

        if (levelIndex > 10)
        {
            levelIndex = 1;
            PlayerPrefs.SetInt(Constants.TAG_LEVEL, levelIndex);
            PlayerPrefs.Save();
            UIManager.Instance.CloseAll();
            UIManager.Instance.OpenUI<MainMenu>().ChangeAnim(Constants.ANIM_FADEIN);
        }
        else
        {
            PlayerPrefs.SetInt(Constants.TAG_LEVEL, levelIndex);
            PlayerPrefs.Save();
        }

        OnReset();
        UIManager.Instance.OpenUI<GamePlay>();
    }
}

