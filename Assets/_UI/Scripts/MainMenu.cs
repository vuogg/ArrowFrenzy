using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : UICanvas
{
    public LevelText levelText;

    private void Update()
    {
        levelText.LevelMainMenu(LevelManager.Instance.levelIndex);
    }

    public void PlayButton()
    {
        LevelManager.Instance.OnStartGame();
        UIManager.Instance.OpenUI<GamePlay>();
        ChangeAnim(Constants.ANIM_FADEOUT);
        Close(0.5f);
    }
}
