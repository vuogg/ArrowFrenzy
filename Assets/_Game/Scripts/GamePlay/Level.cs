using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private int activeArrows = 0;

    private float clearDuration = 3f;

    public List<Target> activeTargets = new();
    public Transform crossbowPosition;

    public void OnInit()
    {
        activeArrows = 0;
        //activeTargets.Clear();

        //if (activeTargets.Count == 0)
        //{
        //    Target[] targets = GetComponentsInChildren<Target>();
        //    for (int i = 0; i < targets.Length; i++)
        //    {
        //        activeTargets.Add(Cache.GetTarget(targets[i].gameObject));
        //    }
        //}

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
    }

    //public bool ShouldTriggerSlowMotion()
    //{
    //    if (activeTargets.Count == 1 && activeTargets[0].hp == 1)
    //    {
    //        return true;
    //    }
    //    return false;
    //}

    private void CheckWinCondition()
    {
        if (activeTargets.Count == 0)
        {
            GameManager.Instance.ChangeState(GameState.Pause);
            AudioManager.Instance.PlaySound(AudioManager.Instance.win);
            UIManager.Instance.OpenUI<Win>().ChangeAnim(Constants.ANIM_SLIDEFROMRIGHT);
            UIManager.Instance.CloseUI<GamePlay>();
            StartCoroutine(IEClearArrowsCouroutine());
            //Time.timeScale = 1f;
        }
    }

    private void CheckLoseCondition()
    {
        if (activeArrows <= 0 && activeTargets.Count > 0)
        {
            for (int i = 0; i < activeTargets.Count; i++)
            {
                activeTargets[i].ChangeAnim(Constants.ANIM_WIN);
            }
            GameManager.Instance.ChangeState(GameState.Pause);
            AudioManager.Instance.PlaySound(AudioManager.Instance.lose);
            UIManager.Instance.OpenUI<Lose>().ChangeAnim(Constants.ANIM_SLIDEFROMLEFT);
            UIManager.Instance.CloseUI<GamePlay>();
        }
    }

    public IEnumerator IEClearArrowsCouroutine()
    {
        yield return Cache.GetWFS(clearDuration);
        SimplePool.CollectAll();
    }
}
