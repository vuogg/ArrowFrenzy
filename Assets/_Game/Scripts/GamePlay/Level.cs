using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Level : MonoBehaviour
{
    //[SerializeField] private List<Target> activeTargets = new();
    [SerializeField] private int activeArrows = 0;

    [SerializeField] private List<Target> activeTargets = new();

    public Transform crossbowPosition;

    public void OnInit()
    {
        activeArrows = 0;
        activeTargets.Clear();

        if (activeTargets.Count == 0)
        {
            Target[] targets = GetComponentsInChildren<Target>();
            for (int i = 0; i < targets.Length; i++)
            {
                activeTargets.Add(Cache.GetTarget(targets[i].gameObject));
            }
        }

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

    private void CheckWinCondition()
    {
        if (activeTargets.Count == 0)
        {
            GameManager.Instance.ChangeState(GameState.Pause);
            UIManager.Instance.OpenUI<Win>().ChangeAnim(Constants.ANIM_SLIDEFROMRIGHT);
            UIManager.Instance.CloseUI<GamePlay>();
            StartCoroutine(ClearArrowsCouroutine());
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
            UIManager.Instance.OpenUI<Lose>().ChangeAnim(Constants.ANIM_SLIDEFROMLEFT);
            UIManager.Instance.CloseUI<GamePlay>();
        }
    }

    public IEnumerator ClearArrowsCouroutine()
    {
        yield return new WaitForSeconds(3f);
        SimplePool.CollectAll();
    }
}
