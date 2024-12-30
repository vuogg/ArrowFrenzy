using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lose : UICanvas
{
    public void RetryButton()
    {
        LevelManager.Instance.OnRetry();
        LevelManager.Instance.OnStartGame();
        Close(0);
    }
}
