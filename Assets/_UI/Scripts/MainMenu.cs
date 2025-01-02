using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : UICanvas
{
    public void PlayButton()
    {
        LevelManager.Instance.OnStartGame();
        UIManager.Instance.OpenUI<GamePlay>();
        Close(0.5f);
        ChangeAnim("fadeOut");
    }
}
