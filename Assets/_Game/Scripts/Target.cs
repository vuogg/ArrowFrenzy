using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] Rigidbody[] ragdollRigidbodies;
    //[SerializeField] private CapsuleCollider capsuleCollider;
    //[SerializeField] private Rigidbody rb;
    [SerializeField] private Animator anim;
    [SerializeField] private int hp = 5;
    [SerializeField] private float force = 20f;
    [SerializeField] private HealthText health;

    //private Vector3 lastHitDirection;
    private string currentAnimName;
    bool isHit = false;
    bool isDead = false;

    private void Start()
    {
        OnInit();
    }

    private void OnInit()
    {
        health.OnInit(hp);
        //if(capsuleCollider == null)
        //{
        //    capsuleCollider = GetComponent<CapsuleCollider>();
        //}
        ChangeAnim("isDance");

        //Lay danh sach rb tu cac xuong
        ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();

        //tat ragdoll
        SetRagdollState(false);
    }    

    public void TakeDamage(int damage)
    {
        if(isDead) return;

        //lastHitDirection = hitDirection;

        if (!isHit)
        {
            ChangeAnim("isIdle");
            isHit = true;
        }
        // target bat ragdoll khi het hp
        hp -= damage;
        DecreaseHealthText(damage);

        if (hp <= 0)
        {
            anim.enabled = false;
            EnableRagdoll();
            ApplyRagdollForce();
            //ApplyRagdollForce(lastHitDirection);
            isDead = true;
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
        }
    }

    public void EnableRagdoll()
    {
        anim.enabled = false;
        SetRagdollState(true);

        //if (capsuleCollider != null)
        //{
        //    capsuleCollider.enabled = false;
        //}

        //if (rb != null)
        //{
        //    rb.isKinematic = true;
        //}    
    }

    private void ApplyRagdollForce()
    {
        foreach(Rigidbody rb in ragdollRigidbodies)
        {
            rb.AddForce(Vector3.forward * force, ForceMode.Impulse);
        }
    }

    private void ChangeAnim(string animName)
    {
        if(anim == null) return;

        if (currentAnimName != animName)
        {
            anim.ResetTrigger(animName);
            currentAnimName = animName;
            anim.SetTrigger(currentAnimName);
        }
    }
}
