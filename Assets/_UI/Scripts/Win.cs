using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Win : UICanvas
{
    public void RetryButton()
    {
        LevelManager.Instance.OnRetry();
        LevelManager.Instance.OnStartGame();
        UIManager.Instance.OpenUI<Win>().ChangeAnim(Constants.ANIM_SLIDERIGHT);
        Close(0.5f);
    }

    public void NextLevelButton()
    {
        LevelManager.Instance.OnNextLevel();
        if(LevelManager.Instance.levelIndex >= 2 && LevelManager.Instance.levelIndex <= 10)
        {
            LevelManager.Instance.OnStartGame();
            UIManager.Instance.OpenUI<Win>().ChangeAnim(Constants.ANIM_SLIDELEFT);
            Close(0.5f);
        }
        else
        {
            AudioManager.Instance.PlayMusic(AudioManager.Instance.mainMenu);
            Close(0.5f);
        }

    }
}
