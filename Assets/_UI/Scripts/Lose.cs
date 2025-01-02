using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lose : UICanvas
{
    public void RetryButton()
    {
        LevelManager.Instance.OnRetry();
        LevelManager.Instance.OnStartGame();
        UIManager.Instance.OpenUI<Lose>().ChangeAnim("slideRight");
        Close(0.5f);
    }

    public void LeaveButton()
    {
        GameManager.Instance.ChangeState(GameState.MainMenu);
        UIManager.Instance.OpenUI<Lose>().ChangeAnim("slideLeft");
        Close(0.5f);
        //UIManager.Instance.CloseAll();
        UIManager.Instance.OpenUI<MainMenu>().ChangeAnim("fadeIn");
    }
}
