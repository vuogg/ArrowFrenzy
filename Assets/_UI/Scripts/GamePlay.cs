using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlay : UICanvas
{
    public void SettingButton()
    {
        GameManager.Instance.ChangeState(GameState.Pause);
        UIManager.Instance.OpenUI<Settings>();
    }
}
