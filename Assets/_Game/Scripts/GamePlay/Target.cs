using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Target : AnimationsController
{
    [SerializeField] public Rigidbody[] ragdollRigidbodies;
    [SerializeField] private int hp = 10;
    [SerializeField] private float force = 30f;
    [SerializeField] private HealthText health;

    public Level levelTargets;
    bool isHit = false;
    bool isDead = false;

    private void Start()
    {
        OnInit();
    } 

    public void OnInit()
    {
        health.OnInit(hp);
        ChangeAnim(Constants.ANIM_DANCE);

        ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();
        SetRagdollState(false);

        isHit = false ;
        isDead = false ;
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        if (!isHit)
        {
            ChangeAnim(Constants.ANIM_IDLE);
            isHit = true;
        }

        hp -= damage;
        DecreaseHealthText(damage);

        if (hp <= 0)
        {
            EnableRagdoll();
            ApplyRagdollForce();
            StartCoroutine(IEKinematicCouroutine());
            isDead = true;

            GameObject levelObject = GameObject.FindGameObjectWithTag(Constants.TAG_LEVEL);
            if (levelObject != null)
            {
                levelTargets = Cache.GetLevel(levelObject);
                if (levelTargets != null)
                {
                    levelTargets.UnregisterTarget(this);
                }
            }
        }
    }

    public void DecreaseHealthText(int value)
    {
        health.CurrentHealth -= value;
    }    

    void SetRagdollState(bool state)
    {
        foreach(Rigidbody rb in ragdollRigidbodies)
        {
            //neu state = true, bat ragdoll
            rb.isKinematic = !state;
            rb.interpolation = RigidbodyInterpolation.Interpolate;
        }
    }

    public void EnableRagdoll()
    {   
        anim.enabled = false;
        SetRagdollState(true);
    }

    private void ApplyRagdollForce()
    {
        foreach(Rigidbody rb in ragdollRigidbodies)
        {
            rb.AddForce(Vector3.forward * force, ForceMode.Impulse);
        }
    }

    private void OnDestroy()
    {
        GameObject levelObject = GameObject.FindGameObjectWithTag(Constants.TAG_LEVEL);
        if (levelObject != null)
        {
            Cache.ClearCache(levelObject);
        }
    }

    public IEnumerator IEKinematicCouroutine()
    {
        yield return new WaitForSeconds(2f);
        foreach(Rigidbody rb in ragdollRigidbodies)
        {
            rb.isKinematic = true;
        }    
    }
}
