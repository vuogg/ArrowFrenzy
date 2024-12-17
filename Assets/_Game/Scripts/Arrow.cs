using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Arrow : GameUnit
{
    private Vector3 velocity;
    public LayerMask reflectLayers;
    public LayerMask buffLayers;
    public LayerMask targetLayers;
    public float arrowSpeed = 10f;

    public void Launch(Vector3 initialVelocity)
    {
        velocity = initialVelocity;
    }

    void Update()
    {
        //chuyen huong mui ten sau khi nay
        transform.position += velocity * Time.deltaTime;

        if (velocity != Vector3.zero) 
        { 
            transform.rotation = Quaternion.LookRotation(velocity);
        }

        CheckCollision(reflectLayers, Reflect);
        CheckCollision(buffLayers, HandleBuff);
        CheckCollision(targetLayers, HitTarget);
    }

    private void CheckCollision(LayerMask layer, System.Action<RaycastHit> collisionAction)
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, velocity.normalized, out hit, velocity.magnitude * Time.deltaTime + 0.1f, layer))
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
            if (hit.collider.CompareTag("Greenx3") || hit.collider.CompareTag("Greenx5") || hit.collider.CompareTag("Greenx7") || hit.collider.CompareTag("Greenx9") || hit.collider.CompareTag("Greenx25"))
            {
                Reflect(hit);
            }
            else if (hit.collider.CompareTag("Yellowx25") || hit.collider.CompareTag("Yellowx50"))
            {
                Destroy(hit.collider.gameObject);
            }
        }
    }
    
    private void HitTarget(RaycastHit hit)
    {
        SimplePool.Despawn(this);
        //Gay sat thuong cho target
        Target targetScript = hit.collider.GetComponent<Target>();
        if (targetScript != null)
        {
            targetScript.TakeDamage(1);
        }
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
}
