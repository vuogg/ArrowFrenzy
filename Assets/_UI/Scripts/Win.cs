using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Win : UICanvas
{
    public void RetryButton()
    {
        LevelManager.Instance.OnRetry();
        LevelManager.Instance.OnStartGame();
        Close(1f);
    }

    public void NextLevelButton()
    {
        LevelManager.Instance.OnNextLevel();
        LevelManager.Instance.OnStartGame();
        Close(1f);
    }
}
