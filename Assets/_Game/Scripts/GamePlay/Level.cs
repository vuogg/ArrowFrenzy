﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private int activeArrows = 0;

    private float clearDuration = 2f;

    public List<Target> activeTargets = new();
    public Transform crossbowPosition;

    public void OnInit()
    {
        activeArrows = 0;

        for (int i = 0; i < activeTargets.Count; i++)
        {
            RegisterTarget(activeTargets[i]);
            activeTargets[i].OnInit();
        }
    }

    public void RegisterTarget(Target target)
    {
        if (!activeTargets.Contains(target))
        {
            activeTargets.Add(target);
        }
    }

    public void UnregisterTarget(Target target)
    {
        if (activeTargets.Contains(target))
        {
            activeTargets.Remove(target);
        }

        CheckWinCondition();
    }

    public void RegisterArrow(Arrow arrow)
    {
        if (arrow != null)
        {
            activeArrows++;
        }
    }

    public void UnregisterArrow(Arrow arrow)
    {
        if (arrow != null && activeArrows > 0)
        {
            activeArrows--;
        }
        CheckLoseCondition();

        if(activeArrows <= 0 && activeTargets.Count == 0)
        {
            OpenUIWin();
        }    
    }

    //FIXME
    public bool ShouldTriggerSlowMotion()
    {
        if (activeTargets.Count == 1 && activeTargets[0].hp == 1)
        {
            return true;
        }
        return false;
    }

    private void CheckWinCondition()
    {
        if (activeTargets.Count == 0)
        {
            StartCoroutine(IESlowBeforeWin());
        }
    }

    private void OpenUIWin()
    {
        GameManager.Instance.ChangeState(GameState.Pause);
        AudioManager.Instance.PlaySound(AudioManager.Instance.win);
        UIManager.Instance.OpenUI<Win>().ChangeAnim(Constants.ANIM_SLIDEFROMRIGHT);
        UIManager.Instance.CloseUI<GamePlay>();
        StartCoroutine(IEClearArrowsCouroutine());
    }    

    private void CheckLoseCondition()
    {
        if (activeArrows <= 0 && activeTargets.Count > 0)
        {
            for (int i = 0; i < activeTargets.Count; i++)
            {
                activeTargets[i].ChangeAnim(Constants.ANIM_WIN);
            }

            StartCoroutine(IEWaitForLoseCouroutine());
        }
    } 

    private IEnumerator IESlowBeforeWin()
    {
        float delayBeforeNormalSpeed = 3f;
        yield return Cache.GetWFSRT(delayBeforeNormalSpeed);

        float elapsedTime = 0f;
        float duration = 2f;

        while (elapsedTime < duration)
        {
            Time.timeScale = Mathf.Lerp(0.05f, 1f, elapsedTime / duration);
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }

        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
    }

    private IEnumerator IEClearArrowsCouroutine()
    {
        yield return Cache.GetWFS(clearDuration);
        SimplePool.CollectAll();
    }

    private IEnumerator IEWaitForLoseCouroutine()
    {
        yield return Cache.GetWFS(clearDuration);

        GameManager.Instance.ChangeState(GameState.Pause);
        AudioManager.Instance.PlaySound(AudioManager.Instance.lose);
        UIManager.Instance.CloseUI<GamePlay>();
        UIManager.Instance.OpenUI<Lose>().ChangeAnim(Constants.ANIM_SLIDEFROMLEFT);
    }
}