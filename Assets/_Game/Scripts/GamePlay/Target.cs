using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Target : AnimationsController
{
    [SerializeField] private Rigidbody[] ragdollRigidbodies;
    [SerializeField] private float force = 30f;
    public HealthBar healthBar;

    private Vector3 impactDirection;
    private float turnOffRagdollDuration = 3f;
    private bool isHit = false;
    private bool isDead = false;

    public int hp = 10;
    public Level levelTargets;

    private void Start()
    {
        OnInit();
    }

    public void OnInit()
    {
        healthBar.SetMaxHealth(hp);

        ChangeAnim(Constants.ANIM_DANCE);

        SetRagdollState(false);

        isHit = false;
        isDead = false;
    }

    public void TakeDamage(int damage, Vector3 arrowDirection)
    {
        if (isDead) return;

        if (!isHit)
        {
            ChangeAnim(Constants.ANIM_IDLE);
            isHit = true;
        }

        hp -= damage;
        healthBar.SetHealth(hp);

        ChangeAnim(Constants.ANIM_TARGETSHAKING);

        impactDirection = arrowDirection;

        if (hp <= 0)
        {
            healthBar.healthBar.enabled = false;
            EnableRagdoll();
            ApplyRagdollForce();
            StartCoroutine(IEKinematicCouroutine());
            isDead = true;

            LevelManager.Instance.currentLevel.UnregisterTarget(this);
        }
    }  

    private void SetRagdollState(bool state)
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
        foreach (Rigidbody rb in ragdollRigidbodies)
        {
            rb.AddForce(impactDirection * force, ForceMode.Impulse);
        }
    }

    private void OnDestroy()
    {
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
