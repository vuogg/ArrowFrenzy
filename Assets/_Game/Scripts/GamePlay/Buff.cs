using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff : AnimationsController
{
    [SerializeField] private BuffText buffText;
    [SerializeField] private Transform arrowSpawnPoint;
    [SerializeField] private Arrow arrowPrefab;
    [SerializeField] private int arrowMultiplier = 3;
    [SerializeField] private float cooldownDuration = 4f;
    [SerializeField] private float spreadAngle = 90f;
    [SerializeField] private float arrowSpeed = 8f;

    private Collider cachedCollider;
    private bool isCooldown = false;

    private void Start()
    {
        cachedCollider = Cache.GetCachedComponent<Collider>(gameObject);
        buffText.OnInit(arrowMultiplier);
    }

    public void ArrowMultiply(Vector3 direction)
    {
        if (isCooldown) return;

        StartCoroutine(IEArrowMultiply(direction));
    }


    private IEnumerator IEArrowMultiply(Vector3 direction)
    {
        //logic nhan mui ten
        float angleStep = spreadAngle / (arrowMultiplier - 1);
        float startAngle = -spreadAngle / 2;

        for(int i = 0; i < arrowMultiplier;i++)
        {
            float angle = startAngle + angleStep * i;
            Quaternion rotation = Quaternion.LookRotation(direction) * Quaternion.Euler(0, angle, 0) * transform.rotation;

            Arrow arrow = SimplePool.Spawn<Arrow>(PoolType.Arrow, arrowSpawnPoint.position, rotation);

            Arrow arrowScript = Cache.GetCachedComponent<Arrow>(arrow.gameObject);
            if (arrowScript != null)
            {
                Vector3 shootDirection = rotation * Vector3.forward;
                arrowScript.Launch(shootDirection.normalized * arrowSpeed);
            }
        }

        //cooldown
        isCooldown = true;
        yield return new WaitForSeconds(cooldownDuration);
        isCooldown = false;
    }
    private void OnDestroy()
    {
        Cache.ClearCache(gameObject);
    }
}
