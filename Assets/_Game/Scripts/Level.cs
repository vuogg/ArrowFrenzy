using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private List<Target> activeTargets = new List<Target>();
    [SerializeField] private int activeArrows = 0;

    public Transform crossbowPosition;

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

    public void RegisterArrow()
    {
        activeArrows++;
    }

    public void UnregisterArrow()
    {
        activeArrows--;

        CheckLoseCondition();
    }

    private void CheckWinCondition()
    {
        if (activeTargets.Count == 0)
        {
            Debug.Log("You Win!");
            // Gọi giao diện hoặc sự kiện xử lý thắng
        }
    }

    private void CheckLoseCondition()
    {
        if (activeArrows <= 0 && activeTargets.Count > 0)
        {
            Debug.Log("You Lose!");
            // Gọi giao diện hoặc sự kiện xử lý thua
        }
    }
}
