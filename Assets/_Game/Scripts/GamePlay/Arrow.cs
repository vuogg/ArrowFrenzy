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

    public int maxReflects = 10;
    public int currentReflects;
    public Level level;
    public LayerMask reflectLayers;
    public LayerMask buffLayers;
    public LayerMask targetLayers;
    public float arrowSpeed = 8f;

    bool isStuck = false;

    private void FixedUpdate()
    {
        if (!isStuck)
        {
            Vector3 nextPoint = TF.position + velocity.normalized * arrowSpeed * Time.deltaTime;

            RaycastHit hit;
            if (Physics.Raycast(TF.position, velocity.normalized, out hit, arrowSpeed * Time.deltaTime + 0.1f, reflectLayers | targetLayers | buffLayers))
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

    private void CheckOutOfBounds()
    {
        if(Mathf.Abs(TF.position.x) > 15f || Mathf.Abs(TF.position.z) > 20f)
        {
            SimplePool.Despawn(this);
            UnregisterArrow();
        }
    }

    public void OnInit()
    {
        isStuck = false;
        currentReflects = 0;
        rb.isKinematic = false;
        col.enabled = true;
        level = null;
    }    

    public void Launch(Vector3 initialVelocity)
    {
        velocity = initialVelocity;
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
            targetScript.TakeDamage(1);
        }

        StickToTarget(hit);
        UnregisterArrow();
    }

    void Reflect(RaycastHit hit)
    {
        //vector phap tuyen va phan xa
        //Phap tuyen tai diem va cham
        Vector3 normal = hit.normal;

        //vector phan xa
        velocity = Vector3.Reflect(velocity, normal);

        //dich chuyen 1 chut de khong va cham lap lai
        TF.position = hit.point + normal * 0.01f;

        currentReflects++;
        if (currentReflects >= maxReflects)
        {
            SimplePool.Despawn(this);
            UnregisterArrow();
        }
    }

    private void StickToTarget(RaycastHit hit)
    {
        rb.isKinematic = true;
        rb.detectCollisions = false;
        TF.SetParent(hit.collider.transform);

        float depth = 0.5f;
        Vector3 hitPosition = hit.point - TF.forward * depth;

        
        //huong mui ten sau khi gam vao
        TF.SetPositionAndRotation(hitPosition, Quaternion.LookRotation(velocity.normalized));
        isStuck = true;
    }

    public void RegisterArrow()
    {
        GameObject levelObject = GameObject.FindGameObjectWithTag(Constants.TAG_LEVEL);
        if (levelObject != null)
        {
            level = levelObject.GetComponent<Level>();
            if (level != null)
            {
                level.RegisterArrow(this);
            }
        }
    }

    public void UnregisterArrow()
    {
        GameObject levelObject = GameObject.FindGameObjectWithTag(Constants.TAG_LEVEL);
        if (levelObject != null)
        {
            level = levelObject.GetComponent<Level>();
            if (level != null)
            {
                level.UnregisterArrow(this);
            }
        }
    }
}
