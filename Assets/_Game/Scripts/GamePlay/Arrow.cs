using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;

public class Arrow : GameUnit
{
    [SerializeField] private BoxCollider col;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private TrailRenderer trailRenderer;

    private Vector3 velocity;

    public Transform hitPos;
    public int maxReflects = 10;
    public int currentReflects;
    public LayerMask reflectLayers;
    public LayerMask buffLayers;
    public LayerMask targetLayers;
    public float arrowSpeed = 10f;

    bool isStuck = false;

    private void FixedUpdate()
    {
        ArrowChecking();
    }

    public void OnInit()
    {
        isStuck = false;
        currentReflects = 0;
        rb.isKinematic = false;
        col.enabled = true;
    }

    public void ArrowChecking()
    {
        if (!isStuck)
        {
            PredictRaycast();

            Vector3 nextPoint = TF.position + arrowSpeed * Time.fixedDeltaTime * velocity.normalized;

            RaycastHit hit;
            if (Physics.Raycast(TF.position, velocity.normalized, out hit, arrowSpeed * Time.fixedDeltaTime + 0.005f, reflectLayers | targetLayers | buffLayers))
            {
                if ((reflectLayers.value & (1 << hit.collider.gameObject.layer)) != 0)
                {
                    Reflect(hit);
                    return;
                }
                else if ((targetLayers.value & (1 << hit.collider.gameObject.layer)) != 0)
                {
                    HitTarget(hit);
                    return;
                }
                else if ((buffLayers.value & (1 << hit.collider.gameObject.layer)) != 0)
                {
                    HandleBuff(hit);
                    return;
                }
            }

            TF.position = nextPoint;

            if (velocity != Vector3.zero)
            {
                TF.rotation = Quaternion.LookRotation(velocity);
            }

            CheckOutOfBounds();
        }
    }

    public void Launch(Vector3 initialVelocity)
    {
        velocity = initialVelocity;
    }

    private void CheckOutOfBounds()
    {
        if(TF.position.x < -13f || TF.position.x > 13f || TF.position.z < -18f || TF.position.z > 18f)
        {
            SimplePool.Despawn(this);
            UnregisterArrow();
        }
    }

    private void PredictRaycast()
    {
        float predictDistance = 2f;
        Ray ray = new(transform.position, velocity.normalized);

        if (Physics.Raycast(ray, out RaycastHit hit, predictDistance, targetLayers) && LevelManager.Instance.currentLevel.ShouldTriggerSlowMotion())
        {
           // StartSlowMotion();
            Time.timeScale = 0.08f;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
        }
    }

    private void HandleBuff(RaycastHit hit)
    {
        Buff buff = Cache.GetBuff(hit.collider);
        if (buff != null)
        {
            buff.ArrowMultiply(velocity.normalized);
            if (hit.collider.CompareTag(Constants.TAG_GREENBUFF))
            {
                buff.ChangeAnim(Constants.ANIM_SHAKING);
                Reflect(hit);
            }
            else if (hit.collider.CompareTag(Constants.TAG_YELLOWBUFF))
            {
                Destroy(hit.collider.gameObject);
            }
        }
    }

    private void HitTarget(RaycastHit hit)
    {
        ParticlePool.Play(ParticleType.Blood, hit.point, Quaternion.LookRotation(hit.normal));

        OnHit targetScript = Cache.GetOnHit(hit.collider);
        if (targetScript != null)
        {
            targetScript.TakeDamage(1, velocity.normalized);
        }

        StickToTarget(hit);
        UnregisterArrow();
    }

    public void Reflect(RaycastHit hit)
    {
        //vector phap tuyen va phan xa
        //Phap tuyen tai diem va cham
        Vector3 normal = hit.normal;

        //vector phan xa
        velocity = Vector3.Reflect(velocity, normal);

        Vector3 sideVector = Vector3.Cross(normal, velocity).normalized;

        float randomAngle = Random.Range(-3f, 3f);
        Quaternion rotation = Quaternion.AngleAxis(randomAngle, sideVector);
        velocity = rotation * velocity;


        currentReflects++;
        if (currentReflects >= maxReflects)
        {
            SimplePool.Despawn(this);
            UnregisterArrow();
        }
    }

    private void StickToTarget(RaycastHit hit)
    {
        col.enabled = false;
        rb.isKinematic = true;
        //rb.detectCollisions = false;

        float depth = 0.45f;
        Vector3 hitPosition = hit.point - TF.forward * depth;
        //Vector3 hitPosition = hit.collider.transform.position - TF.forward * depth;

        TF.SetParent(hit.collider.transform);
        //huong mui ten sau khi gam vao
        TF.SetPositionAndRotation(hitPosition, Quaternion.LookRotation(velocity.normalized));


        isStuck = true;
    }

    public void RegisterArrow()
    {
        LevelManager.Instance.currentLevel.RegisterArrow(this);
    }

    public void UnregisterArrow()
    {
        LevelManager.Instance.currentLevel.UnregisterArrow(this);
    }
}