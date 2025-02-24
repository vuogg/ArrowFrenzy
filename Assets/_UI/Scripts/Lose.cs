using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lose : UICanvas
{
    public void RetryButton()
    {
        LevelManager.Instance.OnRetry();
        LevelManager.Instance.OnStartGame();
        UIManager.Instance.OpenUI<Lose>().ChangeAnim(Constants.ANIM_SLIDERIGHT);
        Close(0.5f);
    }

    public void LeaveButton()
    {
        GameManager.Instance.ChangeState(GameState.MainMenu);
        Close(0);
        //UIManager.Instance.CloseAll();
        UIManager.Instance.OpenUI<MainMenu>().ChangeAnim(Constants.ANIM_FADEIN);
        AudioManager.Instance.PlayMusic(AudioManager.Instance.mainMenu);
    }
}
