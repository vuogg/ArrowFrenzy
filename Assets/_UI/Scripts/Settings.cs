using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : UICanvas
{
    public override void Open()
    {
        base.Open();
        Time.timeScale = 0;
    }

    public override void Close(float delayTime)
    {
        base.Close(delayTime);
    }

    public void ContinueButton()
    {
        ChangeAnim(Constants.ANIM_SLIDEUP);
        Close(0.5f);
        UIManager.Instance.OpenUI<GamePlay>();
        LevelManager.Instance.OnContinue();
        Time.timeScale = 1;
    }

    public void RetryButton()
    {
        ChangeAnim(Constants.ANIM_SLIDEUP);
        Close(0.5f);
        LevelManager.Instance.OnRetry();
        LevelManager.Instance.OnStartGame();
        Time.timeScale = 1;
    }
}
