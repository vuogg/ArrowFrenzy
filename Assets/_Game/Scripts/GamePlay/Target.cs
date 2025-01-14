using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Target : AnimationsController
{
    [SerializeField] private Rigidbody[] ragdollRigidbodies;
    [SerializeField] private int hp = 10;
    [SerializeField] private float force = 40f;
    [SerializeField] private HealthText health;

    private float turnOffRagdollDuration = 2f;
    private bool isHit = false;
    private bool isDead = false;
    public Level levelTargets;

    private void Start()
    {
        OnInit();
    } 

    public void OnInit()
    {
        health.OnInit(hp);
        ChangeAnim(Constants.ANIM_DANCE);

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
        ChangeAnim(Constants.ANIM_TARGETSHAKING);
        DecreaseHealthText(damage);

        if (hp <= 0)
        {
            EnableRagdoll();
            ApplyRagdollForce();
            StartCoroutine(IEKinematicCouroutine());
            isDead = true;

            //TODO
            //GameObject levelObject = GameObject.FindGameObjectWithTag(Constants.TAG_LEVEL);
            //if (levelObject != null)
            //{
            //    levelTargets = Cache.GetLevel(levelObject);
            //    if (levelTargets != null)
            //    {
            //        levelTargets.UnregisterTarget(this);
            //    }
            //}

            LevelManager.Instance.currentLevel.UnregisterTarget(this);
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
        //GameObject levelObject = GameObject.FindGameObjectWithTag(Constants.TAG_LEVEL);
        //if (levelObject != null)
        //{
        //    Cache.ClearCache(levelObject);
        //}

        Cache.ClearCache(gameObject);
    }

    public IEnumerator IEKinematicCouroutine()
    {
        yield return Cache.GetWFS(turnOffRagdollDuration);
        foreach(Rigidbody rb in ragdollRigidbodies)
        {
            rb.isKinematic = true;
        }    
    }
}
