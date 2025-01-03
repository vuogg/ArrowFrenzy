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
        UIManager.Instance.OpenUI<Lose>().ChangeAnim(Constants.ANIM_SLIDELEFT);
        Close(0.5f);
        //UIManager.Instance.CloseAll();
        UIManager.Instance.OpenUI<MainMenu>().ChangeAnim(Constants.ANIM_FADEIN);
    }
}
