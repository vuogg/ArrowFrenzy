using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnHit : MonoBehaviour
{
    [SerializeField] Target target;

    public void TakeDamage(int damage) 
    {
        if (target != null)
        {
            target.TakeDamage(damage);
        }
    }
}
