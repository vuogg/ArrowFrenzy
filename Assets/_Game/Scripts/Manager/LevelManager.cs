using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private CrossBowController crossBowPrefab;
    [SerializeField] private DragController dragControllerPrefab;
    public CrossBowController crossBowInstance;
    public DragController dragControllerInstance;

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
            PlayerPrefs.Save();
            UIManager.Instance.OpenUI<MainMenu>().ChangeAnim(Constants.ANIM_FADEIN);
            AudioManager.Instance.PlayMusic(AudioManager.Instance.mainMenu);
        }
        else
        {
            UIManager.Instance.OpenUI<MainMenu>().ChangeAnim(Constants.ANIM_FADEIN);
            AudioManager.Instance.PlayMusic(AudioManager.Instance.mainMenu);
        }
    }

    public void OnInit()
    {
        Time.timeScale = 1.0f;
        if (crossBowInstance == null)
        {
            crossBowInstance = Instantiate(crossBowPrefab);
        }

        if (dragControllerInstance == null)
        {
            dragControllerInstance = Instantiate(dragControllerPrefab);
        }

        //crossBowController.OnInit();
        crossBowInstance.OnInit();
        dragControllerInstance.OnInit();

        if (currentLevel != null && currentLevel.crossbowPosition != null)
        {
            //crossBowController.transform.SetPositionAndRotation(currentLevel.crossbowPosition.position, currentLevel.crossbowPosition.rotation);
            crossBowInstance.transform.SetPositionAndRotation(currentLevel.crossbowPosition.position, currentLevel.crossbowPosition.rotation);
        }
    }

    public void LoadLevel(int level)
    {
        if (currentLevel != null)
        {
            Destroy(currentLevel.gameObject);
        }


        if (level < levelPrefabs.Length)
        {
            currentLevel = Instantiate(levelPrefabs[level]);
            currentLevel.OnInit();
        }

        //else if (level >= levelPrefabs.Length)
        //{
        //    //levelIndex = 1;
        //    //PlayerPrefs.SetInt("Level", 1);
        //    //currentLevel = Instantiate(levelPrefabs[0]);
        //    UIManager.Instance.CloseAll();
        //    UIManager.Instance.OpenUI<MainMenu>().ChangeAnim(Constants.ANIM_FADEIN);
        //}
    }

    public void OnStartGame()
    {
        LoadLevel(levelIndex - 1);
        GameManager.Instance.ChangeState(GameState.GamePlay);
        AudioManager.Instance.PlayMusic(AudioManager.Instance.gamePlay);
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
        AudioManager.Instance.PlayMusic(AudioManager.Instance.gamePlay);
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

        if (levelIndex <= 10)
        {
            PlayerPrefs.SetInt(Constants.TAG_LEVEL, levelIndex);
            PlayerPrefs.Save();

            UIManager.Instance.OpenUI<GamePlay>();
            AudioManager.Instance.PlayMusic(AudioManager.Instance.gamePlay);

        }

        else if (levelIndex > 10)
        {
            levelIndex = 1;
            PlayerPrefs.SetInt(Constants.TAG_LEVEL, levelIndex);
            PlayerPrefs.Save();

            UIManager.Instance.CloseAll();
            UIManager.Instance.OpenUI<MainMenu>().ChangeAnim(Constants.ANIM_FADEIN);
        }

        OnReset();
    }
}