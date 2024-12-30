using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlay : UICanvas
{
    public void SettingButton()
    {
        LevelManager.Instance.OnPause();
        //GameManager.Instance.ChangeState(GameState.Pause);
        UIManager.Instance.OpenUI<Settings>();
        UIManager.Instance.CloseUI<GamePlay>();
    }
}
