using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnHit : MonoBehaviour
{
    [SerializeField] Target target;

    public void TakeDamage(int damage, Vector3 arrowDirection) 
    {
        if (target != null)
        {
            target.TakeDamage(damage, arrowDirection);
        }
    }

    private void OnDestroy()
    {
        Cache.ClearCache(GetComponent<Collider>());
    }
}
