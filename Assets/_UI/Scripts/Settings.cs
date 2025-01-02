using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : UICanvas
{
    public override void Open()
    {
        base.Open();
        StartCoroutine(MyCouroutine());
    }

    public override void Close(float delayTime)
    {
        base.Close(delayTime);
    }

    public void ContinueButton()
    {
        ChangeAnim("slideUp");
        Close(0.5f);
        UIManager.Instance.OpenUI<GamePlay>();
        LevelManager.Instance.OnContinue();
        Time.timeScale = 1;
    }

    public void RetryButton()
    {
        ChangeAnim("slideUp");
        Close(0.5f);
        LevelManager.Instance.OnRetry();
        LevelManager.Instance.OnStartGame();
        Time.timeScale = 1;
    }

    public IEnumerator MyCouroutine()
    {
        yield return new WaitForSeconds(0.5f);
        Time.timeScale = 0;
    }
}
