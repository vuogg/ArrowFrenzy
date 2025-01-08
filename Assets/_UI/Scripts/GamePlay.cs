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
        UIManager.Instance.OpenUI<Settings>().ChangeAnim(Constants.ANIM_SLIDEDOWN);
        AudioManager.Instance.StopMusic();
        Close(0.5f);
    }
}
