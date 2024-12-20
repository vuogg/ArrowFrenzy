using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Arrow : GameUnit
{
    [SerializeField] private BoxCollider col;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private TrailRenderer trailRenderer;
    private Vector3 velocity;
    public LayerMask reflectLayers;
    public LayerMask buffLayers;
    public LayerMask targetLayers;
    public float arrowSpeed = 10f;

    bool isStuck = false;

    public void Launch(Vector3 initialVelocity)
    {
        velocity = initialVelocity;
    }

    void Update()
    {
        if (!isStuck)
        {
            //chuyen huong mui ten sau khi nay
            transform.position += velocity * Time.deltaTime;

            if (velocity != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(velocity);
            }

            CheckCollision(targetLayers, HitTarget);
            CheckCollision(reflectLayers, Reflect);
            CheckCollision(buffLayers, HandleBuff);
        }
    }

    private void CheckCollision(LayerMask layer, System.Action<RaycastHit> collisionAction)
    {
        RaycastHit hit;
        //Debug.DrawRay(transform.position, velocity.normalized, Color.red, velocity.magnitude * Time.deltaTime + 0.1f);
        if(Physics.Raycast(transform.position, velocity.normalized, out hit, velocity.magnitude * Time.deltaTime + 0.1f, layer))
        {
            //Debug.Log(hit.collider.name);
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
        }

    private void StickToTarget(RaycastHit hit)
    {
        rb.isKinematic = true;
        col.enabled = false;
        trailRenderer.enabled = false;

        // gan tag vao cot song cho de tim
        //Transform spineBone = null;
        //Target targetScript = hit.collider.GetComponent<Target>();
        //if (targetScript != null)
        //{
        //    GameObject spineObject = GameObject.FindGameObjectWithTag("Spine");
        //    if (spineObject != null)
        //    {
        //        spineBone = spineObject.transform;
        //    }
        //}

        ////Gan mui ten vao xuong song hoac collider neu khong tim thay tag
        //if (spineBone != null)
        //{
        //    transform.SetParent(spineBone);
        //}
        //else
        //{
        //    Debug.LogError(hit.collider.name);
        //    transform.SetParent(hit.collider.transform);
        //}

        //Debug.LogError(hit.collider.name);
        transform.SetParent(hit.collider.transform);

        //vi tri mui ten sau khi gam vao
        Vector3 hitPosition = hit.point + hit.normal * 0.3f;
        transform.position = hitPosition;

        //huong mui ten sau khi gam vao
        transform.rotation = Quaternion.LookRotation(velocity.normalized);

        isStuck = true;
    }
}
