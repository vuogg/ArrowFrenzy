using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlay : UICanvas
{
    public LevelText levelText;

    private void Update()
    {
        levelText.LevelGamePlay(LevelManager.Instance.levelIndex);
    }

    public void SettingButton()
    {
        LevelManager.Instance.OnPause();
        //GameManager.Instance.ChangeState(GameState.Pause);
        UIManager.Instance.OpenUI<Settings>().ChangeAnim("slideDown");
        Close(0.5f);
    }
}
