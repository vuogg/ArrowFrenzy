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

    public int maxReflects = 20;
    public int currentReflects;
    private Vector3 velocity;
    public Level level;
    public LayerMask reflectLayers;
    public LayerMask buffLayers;
    public LayerMask targetLayers;
    public float arrowSpeed = 10f;

    bool isStuck = false;

    //private void Start()
    //{
    //    OnInit();
    //}

    private void Update()
    {
        if (!isStuck)
        {
            float stepDistance = velocity.magnitude * Time.deltaTime;
            Vector3 startPosition = transform.position;

            for (float i = 0; i < stepDistance; i += 0.1f)
            {
                Vector3 checkPosition = startPosition + velocity.normalized * i;
                CheckCollision(checkPosition, reflectLayers, Reflect);
                CheckCollision(checkPosition, targetLayers, HitTarget);
                CheckCollision(checkPosition, buffLayers, HandleBuff);
            }

            transform.position += velocity * Time.deltaTime;

            if (velocity != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(velocity);
            }

            CheckOutOfBounds();
        }
    }

    private void CheckOutOfBounds()
    {
        if(Mathf.Abs(transform.position.x) > 15f || Mathf.Abs(transform.position.z) > 20f)
        {
            Debug.Log("Despawn do bay ra khoi map");
            SimplePool.Despawn(this);
            UnregisterArrow();
        }
    }

    public void OnInit()
    {
        isStuck = false;
        //RegisterArrow();
        currentReflects = 0;
        rb.isKinematic = false;
        col.enabled = true;
        //trailRenderer.enabled = true;
        level = null;
    }    

    public void Launch(Vector3 initialVelocity)
    {
        velocity = initialVelocity;
    }

    private void CheckCollision(Vector3 position, LayerMask layer, System.Action<RaycastHit> collisionAction)
    {
        RaycastHit hit;
        float checkDistance = velocity.magnitude * Time.deltaTime + 0.01f;  // Điều chỉnh khoảng cách raycast
        if (Physics.Raycast(position, velocity.normalized, out hit, checkDistance, layer))
        {
            collisionAction(hit);
        }
    }

    private void HandleBuff(RaycastHit hit)
    {
        Buff buff = hit.collider.GetComponent<Buff>();
        if (buff != null)
        {
            buff.ArrowMultiply(velocity.normalized);
            if (hit.collider.CompareTag("GreenBuff"))
            {
                Reflect(hit);
            }
            else if (hit.collider.CompareTag("YellowBuff"))
            {
                Destroy(hit.collider.gameObject);
            }
        }
    }
    
    private void HitTarget(RaycastHit hit)
    {
        //SimplePool.Despawn(this);
        //Gay sat thuong cho target
        //Target targetScript = hit.collider.GetComponent<Target>();
        //if (targetScript != null)
        //{
        //Vector3 spawnPosition = targetScript.transform.position;
        //Quaternion rotation = Quaternion.LookRotation(hitDirection);

        //SimplePool.Spawn<Arrow>(PoolType.Arrow, spawnPosition, rotation);
        //Vector3 hitDirection = velocity.normalized;
        //targetScript.TakeDamage(1, hitDirection);
        //}
        OnHit targetScript = hit.collider.GetComponent<OnHit>();
        if(targetScript != null)
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
        transform.position = hit.point + normal * 0.01f;

        currentReflects++;
        if(currentReflects >= maxReflects)
        {
            SimplePool.Despawn(this);
            UnregisterArrow();
        }
    }

    private void StickToTarget(RaycastHit hit)
    {
        rb.isKinematic = true;
        //col.enabled = false;
        //if (trailRenderer != null)
        //{
        //    trailRenderer.enabled = false;
        //}
        //else
        //{
        //    Debug.LogError("TrailRenderer is null when trying to disable it.");
        //}

        //Debug.LogError(hit.collider.name);
        transform.SetParent(hit.collider.transform);

        //vi tri mui ten sau khi gam vao
        //Vector3 hitPosition = hit.point + hit.normal * 0.3f;
        //Vector3 hitPosition = hit.collider.transform.position;
        float depth = 0.3f;
        Vector3 hitPosition = hit.point - transform.forward * depth;

        transform.position = hitPosition;

        //huong mui ten sau khi gam vao
        transform.rotation = Quaternion.LookRotation(velocity.normalized);

        isStuck = true;
    }

    public void RegisterArrow()
    {
        GameObject levelObject = GameObject.FindGameObjectWithTag("Level");
        //levelArrow = GetComponent<Level>();
        if (levelObject != null)
        {
            level = levelObject.GetComponent<Level>();
            if (level != null)
            {
                level.RegisterArrow(this);
            }
        }
        else
        {
            Debug.Log("level null");
        }
    }

    public void UnregisterArrow()
    {
        GameObject levelObject = GameObject.FindGameObjectWithTag("Level");
        //levelArrow = GetComponent<Level>();
        if (levelObject != null)
        {
            level = levelObject.GetComponent<Level>();
            if (level != null)
            {
                level.UnregisterArrow(this);
            }
        }
        else
        {
            Debug.Log("level null");
        }
    }
}
