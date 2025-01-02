using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Level : MonoBehaviour
{
    [SerializeField] private List<Target> activeTargets = new();
    [SerializeField] private int activeArrows = 0;

    public Transform crossbowPosition;

    public void OnInit()
    {
        activeArrows = 0;
        activeTargets.Clear();

        Target[] targets = GetComponentsInChildren<Target>();
        foreach (var target in targets)
        {
            RegisterTarget(target);
            target.OnInit();
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
        if (arrow != null)
        {
            activeArrows--;
        }
        CheckLoseCondition();
    }

    private void CheckWinCondition()
    {
        //if (activeTargets.Count == 0)
        //{
        //    Debug.Log("You Win!");
        //    GameManager.Instance.ChangeState(GameState.Pause);
        //    UIManager.Instance.OpenUI<Win>();
        //    UIManager.Instance.CloseUI<GamePlay>();
        //    LevelManager.Instance.OnReset();
        //}
        if (activeTargets.Count == 0)
        {
            Debug.Log("You Win!");
            GameManager.Instance.ChangeState(GameState.Pause);
            UIManager.Instance.OpenUI<Win>().ChangeAnim("slideFromRight");
            UIManager.Instance.CloseUI<GamePlay>();
            StartCoroutine(ClearArrowsCouroutine());
        }
    }

    private void CheckLoseCondition()
    {
        if (activeArrows <= 0 && activeTargets.Count > 0)
        {
            Debug.Log("You Lose!");
            Target[] targets = GetComponentsInChildren<Target>();
            foreach (var target in targets)
            {
                target.ChangeAnim("isWin");
            }
            GameManager.Instance.ChangeState(GameState.Pause);
            UIManager.Instance.OpenUI<Lose>().ChangeAnim("slideFromLeft");
            UIManager.Instance.CloseUI<GamePlay>();
        }
    }

    public IEnumerator ClearArrowsCouroutine()
    {
        yield return new WaitForSeconds(3f);
        SimplePool.CollectAll();
    }
}
