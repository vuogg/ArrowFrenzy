using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff : MonoBehaviour
{
    [SerializeField] private float cooldownDuration = 5f;
    [SerializeField] private int arrowMultiplier = 3;
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private float spreadAngle = 90f;
    [SerializeField] private Transform arrowSpawnPoint;
    [SerializeField] private float arrowSpeed = 10f;
    private bool isCooldown = false;

    public void ArrowMultiply(Vector3 direction)
    {
        if (isCooldown) return;

        StartCoroutine(ArrowMultiplyCoroutine(direction));
    }
    
    private IEnumerator ArrowMultiplyCoroutine(Vector3 direction)
    {
        //logic nhan mui ten
        float angleStep = spreadAngle / (arrowMultiplier - 1);
        float startAngle = -spreadAngle / 2;

        for(int i = 0; i < arrowMultiplier;i++)
        {
            float angle = startAngle + angleStep * i;
            Quaternion rotation = Quaternion.LookRotation(direction) * Quaternion.Euler(0, angle, 0) * transform.rotation;

            GameObject newArrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, rotation);

            Arrow arrowScript = newArrow.GetComponent <Arrow>();
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
}
