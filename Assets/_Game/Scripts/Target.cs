using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private int hp = 5;

    public void TakeDamage(int damage)
    {
        //destroy target khi het hp
        hp -= damage;
        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }
}
