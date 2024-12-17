using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] Rigidbody[] ragdollRigidbodies;
    [SerializeField] private CapsuleCollider capsuleCollider;
    [SerializeField] private Animator anim;
    [SerializeField] private int hp = 5;
    [SerializeField] private float force = 20f;
    [SerializeField] Transform stickArrow;

    private Vector3 lastHitDirection;
    private string currentAnimName;
    bool isHit = false;
    bool isDead = false;

    private void Start()
    {
        if(capsuleCollider == null)
        {
            capsuleCollider = GetComponent<CapsuleCollider>();
        }
        ChangeAnim("isDance");

        //Lay danh sach rb tu cac xuong
        ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();

        //tat ragdoll
        SetRagdollState(false);
    }

    public void TakeDamage(int damage, Vector3 hitDirection)
    {
        if(isDead) return;

        lastHitDirection = hitDirection;

        //Vector3 spawnPosition = stickArrow.position;

        //Quaternion rotation = Quaternion.LookRotation(hitDirection);

        //SimplePool.Spawn<Arrow>(PoolType.Arrow, spawnPosition, rotation);

        if (!isHit)
        {
            ChangeAnim("isIdle");
            isHit = true;
        }
        // target bat ragdoll khi het hp
        hp -= damage;
        if (hp <= 0)
        {
            anim.enabled = false;
            EnableRagdoll();
            ApplyRagdollForce(lastHitDirection);
        }
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

        if(capsuleCollider != null)
        {
            capsuleCollider.enabled = false;
        }

        foreach (Transform child in transform)
        {
            if (child.CompareTag("Arrow"))
            {
                child.SetParent(null); // Nếu cần, gỡ gắn kết mũi tên khỏi target
            }
        }
    }

    private void ApplyRagdollForce(Vector3 Direction)
    {
        foreach(Rigidbody rb in ragdollRigidbodies)
        {
            rb.AddForce(Direction * force, ForceMode.Impulse);
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
